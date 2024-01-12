using Core.Entities;
using Core.Entities.Identity;

namespace Core.Dtos.TimeRecordDtos;

public class TimeRecordReturnDto
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Duration { get; set; }
    public string? Description { get; set; }
    // nav props
    public virtual Project? Project { get; set; }
    public virtual AppUser? AppUser { get; set; }
    // FK
    public Guid? ProjectId { get; set; }
    public string? AppUserId { get; set; }
}