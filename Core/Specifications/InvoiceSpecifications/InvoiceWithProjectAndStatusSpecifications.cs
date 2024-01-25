using System.Linq.Expressions;
using Core.Dtos.InvoiceDtos;
using Core.Entities;

namespace Core.Specifications.InvoiceSpecifications;

public class InvoiceWithProjectAndStatusSpecifications : BaseSpecification<Invoice>
{
    public InvoiceWithProjectAndStatusSpecifications(InvoiceSpecParams invoiceSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(invoiceSpecParams.Search) || x.InvoiceNumber!.ToLower().Contains(invoiceSpecParams.Search)) &&
            (!invoiceSpecParams.ProjectId.HasValue || x.ProjectId == invoiceSpecParams.ProjectId) &&
            (!invoiceSpecParams.WorkspaceId.HasValue || x.WorkspaceId == invoiceSpecParams.WorkspaceId ) &&
            (!invoiceSpecParams.InvoiceStatusId.HasValue || x.InvoiceStatusId == invoiceSpecParams.InvoiceStatusId)
        )
    {
        AddInclude(x => x.Project!);
        AddInclude(x => x.Status!);
        AddOrderBy(x => x.InvoiceNumber!);
        ApplyPaging(invoiceSpecParams.PageSize * (invoiceSpecParams.PageIndex - 1), invoiceSpecParams.PageSize);
    }
    
    public InvoiceWithProjectAndStatusSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.Project!);
        AddInclude(x => x.Status!);
    }
}