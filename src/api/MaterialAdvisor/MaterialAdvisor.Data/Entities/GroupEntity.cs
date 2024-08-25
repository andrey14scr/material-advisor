﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialAdvisor.Data.Entities;

[Table("Groups")]
public class GroupEntity
{
    public Guid Id { get; set; }

    public Guid? ParentGroupId { get; set; }

    public virtual ICollection<GroupEntity> ParentGroups { get; set; } = [];

    public Guid OwnerId { get; set; }

    public virtual UserEntity Owner { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = [];
}