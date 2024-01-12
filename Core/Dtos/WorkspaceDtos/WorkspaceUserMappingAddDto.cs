using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.WorkspaceDtos;

public class WorkspaceUserMappingAddDto
{
    // FK
    [Required]
    public Guid WorkspaceId { get; set; }
    [Required]
    public string? AppUserId { get; set; }
}