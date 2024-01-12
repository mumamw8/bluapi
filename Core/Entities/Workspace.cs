using System.ComponentModel.DataAnnotations;
using Core.Entities.Identity;

namespace Core.Entities;

public class Workspace : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    // nav props
    public virtual AppUser? AppUser { get; set; }
    public virtual ICollection<WorkspaceUserMapping>? WorkspaceUserMappings { get; set; }
    // FK
    [Required]
    public string? AppUserId { get; set; }
}