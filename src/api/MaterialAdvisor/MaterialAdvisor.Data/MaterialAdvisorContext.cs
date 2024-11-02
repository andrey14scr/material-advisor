using MaterialAdvisor.Data.Entities;
using MaterialAdvisor.Data.Enums;

using Microsoft.EntityFrameworkCore;

namespace MaterialAdvisor.Data;

public class MaterialAdvisorContext : DbContext
{
    public DbSet<AnswerEntity> Answers { get; set; }
    public DbSet<AnswerGroupEntity> AnswerGroups { get; set; }
    public DbSet<LanguageEntity> Languages { get; set; }
    public DbSet<LanguageTextEntity> LanguageTexts { get; set; }
    public DbSet<QuestionEntity> Questions { get; set; }
    public DbSet<TopicEntity> Topics { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<GroupEntity> Groups { get; set; }
    public DbSet<KnowledgeCheckEntity> KnowledgeChecks { get; set; }
    public DbSet<SubmittedAnswerEntity> SubmittedAnswers { get; set; }
    public DbSet<VerifiedAnswerEntity> VerifiedAnswers { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<AttemptEntity> Attempts { get; set; }

    public MaterialAdvisorContext(DbContextOptions<MaterialAdvisorContext> options) : base(options)
    {
    }

    public MaterialAdvisorContext()
    {

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        ChangeTracker.Clear();
        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=MaterialAdvisorDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var languages = Enum.GetValues(typeof(Language))
            .Cast<Language>()
            .Select(l => new LanguageEntity { Id = l, Name = l.ToString(), Code = string.Empty })
            .ToList();

        modelBuilder.Entity<LanguageEntity>().HasData(languages);

        modelBuilder.Entity<GroupEntity>()
            .HasMany(x => x.Users)
            .WithMany(x => x.Groups);

        modelBuilder.Entity<GroupEntity>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.CreatedGroups)
            .HasForeignKey(x => x.OwnerId);
    }
}