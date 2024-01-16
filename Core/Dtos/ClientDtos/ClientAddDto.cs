using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ClientDtos;

public class ClientAddDto
{
    [Required]
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    // FK
    public Guid WorkspaceId { get; set; }
    public Guid? ContactId { get; set; }
}