namespace Core.Dtos.TimeRecordDtos;

public class TimeRecordAddDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Duration { get; set; }
    public string? Description { get; set; }
    // FK
    public Guid ProjectId { get; set; }
    // public string? AppUserId { get; set; } get from session
}