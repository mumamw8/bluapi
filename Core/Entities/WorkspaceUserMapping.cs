using System.ComponentModel.DataAnnotations;
using Core.Entities.Identity;

namespace Core.Entities;

public class WorkspaceUserMapping : BaseEntity
{
    // nav props
    public virtual Workspace? Workspace { get; set; }
    public virtual AppUser? AppUser { get; set; }
    // FK
    [Required]
    public Guid WorkspaceId { get; set; }
    [Required]
    public string? AppUserId { get; set; }
}