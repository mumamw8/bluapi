using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos.ClientDtos;

public class ClientReturnDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileImageUrl { get; set; }
    // FK
    public Guid WorkspaceId { get; set; }
    public Guid ContactId { get; set; }
    // nav props
    public virtual Workspace? Workspace { get; set; }
    public virtual Contact? Contact { get; set; }
}