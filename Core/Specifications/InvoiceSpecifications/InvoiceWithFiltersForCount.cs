using System.Linq.Expressions;
using Core.Dtos.InvoiceDtos;
using Core.Entities;

namespace Core.Specifications.InvoiceSpecifications;

public class InvoiceWithFiltersForCount : BaseSpecification<Invoice>
{
    public InvoiceWithFiltersForCount(InvoiceSpecParams invoiceSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(invoiceSpecParams.Search) || x.InvoiceNumber!.ToLower().Contains(invoiceSpecParams.Search)) &&
            (!invoiceSpecParams.ProjectId.HasValue || x.ProjectId == invoiceSpecParams.ProjectId) &&
            (!invoiceSpecParams.WorkspaceId.HasValue || x.WorkspaceId == invoiceSpecParams.WorkspaceId) &&
            (!invoiceSpecParams.InvoiceStatusId.HasValue || x.InvoiceStatusId == invoiceSpecParams.InvoiceStatusId)
        )
    {
    }
}