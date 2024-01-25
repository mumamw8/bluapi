using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.ContactDtos;

public class ContactAddDto
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid WorkspaceId { get; set; }
}