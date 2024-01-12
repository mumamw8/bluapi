using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class InvoiceItem : BaseEntity
{
    [Required]
    public string? Description { get; set; }
    public string? Details { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
    // nav props
    public virtual Invoice? Invoice { get; set; }
    // FK
    public Guid InvoiceId { get; set; }
}