using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MaterialAdvisor.Data.Entities.Anstraction;
namespace MaterialAdvisor.Data.Entities;

[Table("RefreshTokens")]
public class RefreshTokenEntity : IEntity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Value { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }

    public bool IsRevoked { get; set; }
}