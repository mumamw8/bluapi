using Core.Dtos.InvoiceDtos;
using Core.Entities;

namespace Core.Dtos.ReceiptDtos;

public class ReceiptReturnDto
{
    public Guid Id { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? ReceiptFileUrl { get; set; }
    // nav props
    public Invoice? Invoice { get; set; }
    // FK
    public Guid InvoiceId { get; set; }
}