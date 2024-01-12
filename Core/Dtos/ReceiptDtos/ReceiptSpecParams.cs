namespace Core.Dtos.ReceiptDtos;

public class ReceiptSpecParams
{
    private const int MaxPageSize = 50;
    
    public int PageIndex { get; set; } = 1;
    private int _pageSize { get; set; } = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public Guid? InvoiceId { get; set; }
    public string? Sort { get; set; }
    private string? _search;
    public string? Search
    {
        get => _search;
        set => _search = value.ToLower();
    }
}