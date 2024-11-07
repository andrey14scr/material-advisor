using FluentValidation;

using MaterialAdvisor.AI.Configuration;
using MaterialAdvisor.API;
using MaterialAdvisor.API.Middleware;
using MaterialAdvisor.API.Options;
using MaterialAdvisor.API.Services;
using MaterialAdvisor.Application.Configuration;
using MaterialAdvisor.Application.Quartz.Configuration;
using MaterialAdvisor.Application.QueueStorage.Configuration;
using MaterialAdvisor.SignalR;
using MaterialAdvisor.SignalR.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;

using System.Text;

const string ApiCorsPolicy = "ApiCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options => options.AddPolicy(ApiCorsPolicy, builder =>
{
    builder
        .WithOrigins("http://localhost", "http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
}));

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<TokensGenerator>();

var configuration = builder.Configuration;
builder.Services.ConfigureServices(configuration);
builder.Services.ConfigureQueueStorage(configuration);
builder.Services.ConfigureSignalR(configuration);
builder.Services.ConfigureQuartz(configuration);
builder.Services.ConfigureOpenAIAssistants(configuration);

var jwtOptionPath = Constants.Configuration.AuthSection;
builder.Services.Configure<AuthOptions>(configuration.GetSection(jwtOptionPath));
var jwtOptions = configuration.GetSection(jwtOptionPath).Get<AuthOptions>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && 
                    (path.StartsWithSegments(SignalRConstants.TopicGenerationHubName) || 
                        path.StartsWithSegments(SignalRConstants.AnswerVerificationHubName)))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(ApiCorsPolicy);

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<EndpointLogMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureHubs();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.Run();
