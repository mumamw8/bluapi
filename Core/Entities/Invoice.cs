using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.Entities;

public class Invoice : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string? InvoiceNumber { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    [Required]
    public string? From { get; set; }
    [Required]
    public string? To { get; set; }
    public string? ExtraNotes { get; set; }
    public string? Terms { get; set; }
    public string? LogoUrl { get; set; }
    
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    
    // nav props
    public virtual Project? Project { get; set; }
    public virtual InvoiceStatus? Status { get; set; }
    public virtual ICollection<InvoiceItem>? InvoiceItems { get; set; }
    // FK
    public Guid ProjectId { get; set; }
    [Required]
    public int InvoiceStatusId { get; set; }
}

public class InvoiceStatus
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    
    // nav props
    // public virtual ICollection<Invoice>? Invoices { get; set; }
}