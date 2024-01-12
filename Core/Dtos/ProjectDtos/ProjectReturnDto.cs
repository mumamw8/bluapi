using Core.Entities;

namespace Core.Dtos.ProjectDtos;

public class ProjectReturnDto
{
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    // nav props
    public virtual Client? Client { get; set; }
    public ProjectStatus? Status { get; set; }
    // FK
    public Guid ClientId { get; set; }
}