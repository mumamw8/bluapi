using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos.ReceiptDtos;

public class ReceiptAddDto
{
    [Required]
    public string? ReceiptNumber { get; set; }
    public string? ReceiptFileUrl { get; set; }
    // FK
    public Guid InvoiceId { get; set; }
}