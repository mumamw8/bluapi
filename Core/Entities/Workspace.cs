using System.Collections;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Identity;

namespace Core.Entities;

public class Workspace : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    // nav props
    public virtual AppUser? AppUser { get; set; }
    public virtual ICollection<WorkspaceUserMapping>? WorkspaceUserMappings { get; set; }
    public virtual ICollection<Client>? Clients { get; set; }
    public virtual ICollection<Estimate>? Estimates { get; set; }
    public virtual ICollection<Invoice>? Invoices { get; set; }
    public virtual ICollection<Project>? Projects { get; set; }
    public virtual ICollection<Receipt>? Receipts { get; set; }
    public virtual ICollection<TimeRecord>? TimeRecords { get; set; }
    public virtual ICollection<Contact>? Contacts { get; set; }
    
    // FK
    [Required]
    public string? AppUserId { get; set; }
}