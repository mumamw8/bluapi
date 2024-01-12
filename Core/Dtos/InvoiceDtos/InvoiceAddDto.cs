using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos.InvoiceDtos;

public class InvoiceAddDto
{
    [Required]
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
    // FK
    public Guid ProjectId { get; set; }
    [Required]
    public int InvoiceStatusId { get; set; }
    public List<InvoiceItem>? InvoiceItems { get; set; }
}