using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos.InvoiceDtos;

public class InvoiceItemAddDto
{
    [Required]
    public string? Description { get; set; }
    public string? Details { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    // FK
    // public Guid InvoiceId { get; set; }
}