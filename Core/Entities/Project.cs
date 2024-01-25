using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.Entities;

public class Project : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Start { get; set; } = null;
    public DateTime? End { get; set; } = null;
    
    // nav props
    public virtual Client? Client { get; set; }
    public ProjectStatus? Status { get; set; }
    // FK
    public Guid ClientId { get; set; }
    [Required]
    public int ProjectStatusId { get; set; }
    [Required]
    public Guid WorkspaceId { get; set; }
    public virtual Workspace? Workspace { get; set; }
}

public class ProjectStatus
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
}