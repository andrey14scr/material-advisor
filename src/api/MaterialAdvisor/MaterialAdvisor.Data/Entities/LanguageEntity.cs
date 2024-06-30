using MaterialAdvisor.Data.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Languages")]
public class LanguageEntity
{
    public LanguageType Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
}
