using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity;

public class AppUser : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }
    [MaxLength(1028)]
    public string? Bio { get; set; }
    [MaxLength(1028)]
    public string? ProfilePictureUrl { get; set; }
    
    // nav props
    public virtual ICollection<Workspace>? Workspaces { get; set; }
    public virtual ICollection<WorkspaceUserMapping>? WorkspaceUserMappings { get; set; }
}