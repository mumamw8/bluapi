using System.Linq.Expressions;
using Core.Dtos.ReceiptDtos;
using Core.Entities;

namespace Core.Specifications.ReceiptSpecifications;

public class ReceiptWithInvoiceSpecifications : BaseSpecification<Receipt>
{
    public ReceiptWithInvoiceSpecifications(ReceiptSpecParams receiptSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(receiptSpecParams.Search) || x.ReceiptNumber!.ToLower().Contains(receiptSpecParams.Search)) &&
            (!receiptSpecParams.WorkspaceId.HasValue || x.WorkspaceId == receiptSpecParams.WorkspaceId) &&
            (!receiptSpecParams.InvoiceId.HasValue || x.InvoiceId == receiptSpecParams.InvoiceId)
        )
    {
        AddInclude(x => x.Invoice!);
        AddOrderBy(x => x.ReceiptNumber!);
        ApplyPaging(receiptSpecParams.PageSize * (receiptSpecParams.PageIndex - 1), receiptSpecParams.PageSize);
    }
    
    public ReceiptWithInvoiceSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.Invoice!);
    }
}