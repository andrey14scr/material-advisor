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
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<SubmittedAnswerEntity> SubmittedAnswers { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<AttemptEntity> Attempts { get; set; }
    public DbSet<GroupRoleEntity> GroupRoles { get; set; }

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

        var roles = Enum.GetValues(typeof(RoleType))
            .Cast<RoleType>()
            .Select(r => new RoleEntity { Id = r, Name = r.ToString() })
            .ToList();

        var permissions = Enum.GetValues(typeof(PermissionType))
            .Cast<PermissionType>()
            .Select(p => new PermissionEntity { Id = p, Name = p.ToString() })
            .ToList();

        modelBuilder.Entity<LanguageEntity>().HasData(languages);
        modelBuilder.Entity<RoleEntity>().HasData(roles);
        modelBuilder.Entity<PermissionEntity>().HasData(permissions);

        modelBuilder.Entity<RoleEntity>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesPermissions",
                    r => r.HasOne<PermissionEntity>().WithMany().HasForeignKey("PermissionId"),
                    l => l.HasOne<RoleEntity>().WithMany().HasForeignKey("RoleId"),
                    je =>
                    {
                        je.HasKey("RoleId", "PermissionId");
                        je.HasData(
                            new { RoleId = RoleType.Teacher, PermissionId = PermissionType.ReadTopic },
                            new { RoleId = RoleType.Teacher, PermissionId = PermissionType.EditTopic },
                            new { RoleId = RoleType.Teacher, PermissionId = PermissionType.ReadKnowledgeCheck },
                            new { RoleId = RoleType.Teacher, PermissionId = PermissionType.EditKnowledgeCheck },

                            new { RoleId = RoleType.Student, PermissionId = PermissionType.ReadKnowledgeCheck },
                            new { RoleId = RoleType.Student, PermissionId = PermissionType.ReadTopic },
                            new { RoleId = RoleType.Student, PermissionId = PermissionType.PassKnowledgeCheck },

                            new { RoleId = RoleType.Admin, PermissionId = PermissionType.EditGroups },
                            new { RoleId = RoleType.Admin, PermissionId = PermissionType.ReadGroups }
                        );
                    });

        modelBuilder.Entity<GroupEntity>(entity =>
        {
            entity.HasOne(x => x.ParentGroup)
                .WithMany(x => x.ChildrenGroups)
                .HasForeignKey(x => x.ParentGroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}