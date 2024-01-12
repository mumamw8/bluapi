using System.Linq.Expressions;
using Core.Dtos.ReceiptDtos;
using Core.Entities;

namespace Core.Specifications.ReceiptSpecifications;

public class ReceiptWithFiltersForCount : BaseSpecification<Receipt>
{
    public ReceiptWithFiltersForCount(ReceiptSpecParams receiptSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(receiptSpecParams.Search) || x.ReceiptNumber!.ToLower().Contains(receiptSpecParams.Search)) &&
            (!receiptSpecParams.InvoiceId.HasValue || x.InvoiceId == receiptSpecParams.InvoiceId)
        )
    {
    }
}