using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Receipt : BaseEntity
{
    [MaxLength(255)]
    [Required]
    public string? ReceiptNumber { get; set; }
    public string? ReceiptFileUrl { get; set; }
    
    // nav props
    public Invoice? Invoice { get; set; }
    // FK
    public Guid InvoiceId { get; set; }
    [Required]
    public Guid WorkspaceId { get; set; }
    public virtual Workspace? Workspace { get; set; }
}