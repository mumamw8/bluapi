using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Entities.Identity;

namespace Core.Dtos.WorkspaceDtos;

public class WorkspaceAddDto
{
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    // FK
    // public string? AppUserId { get; set; } get from session
}