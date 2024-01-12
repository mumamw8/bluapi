using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ProjectDtos;

public class ProjectAddDto
{
    [Required]
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    // FK
    public Guid ClientId { get; set; }
    public int ProjectStatusId { get; set; } = 1;
}