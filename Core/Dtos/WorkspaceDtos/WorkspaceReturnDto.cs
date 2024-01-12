using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.Identity;

namespace Core.Dtos.WorkspaceDtos;

public class WorkspaceReturnDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    // nav props
    public virtual ICollection<WorkspaceUserMapping>? WorkspaceUserMappings { get; set; }
    // FK
    public string? AppUserId { get; set; }
}