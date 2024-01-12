namespace Core.Dtos.InvoiceDtos;

public class InvoiceItemReturnDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? Details { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    // FK
    public Guid InvoiceId { get; set; }
}