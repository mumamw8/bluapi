using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Contact : BaseEntity
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    
    // FK
    [Required]
    public Guid WorkspaceId { get; set; }

    // nav props
    public virtual ICollection<Client>? Clients { get; set; }
    public virtual Workspace? Workspace { get; set; }
}