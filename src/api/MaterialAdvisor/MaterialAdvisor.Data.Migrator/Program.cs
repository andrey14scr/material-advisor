using MaterialAdvisor.Data;

using Microsoft.EntityFrameworkCore;

Console.WriteLine("Applying migrations...");

var optionsBuilder = new DbContextOptionsBuilder<MaterialAdvisorContext>();
optionsBuilder.UseSqlServer("Server=.;Database=MaterialAdvisorDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

using (var context = new MaterialAdvisorContext(optionsBuilder.Options))
{
    context.Database.Migrate();
}

Console.WriteLine("Migrations applied successfully.");