using System.Linq.Expressions;
using Core.Dtos.ContactDtos;
using Core.Entities;

namespace Core.Specifications;

public class ContactSpecification : BaseSpecification<Contact>
{
    public ContactSpecification(ContactSpecParams contactSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(contactSpecParams.Search) || x.FirstName!.ToLower().Contains(contactSpecParams.Search)) &&
            (string.IsNullOrEmpty(contactSpecParams.Search) || x.LastName!.ToLower().Contains(contactSpecParams.Search)) &&
            (!contactSpecParams.WorkspaceId.HasValue || x.WorkspaceId == contactSpecParams.WorkspaceId)
        )
    {
        AddOrderBy(x => x.LastName!);
        ApplyPaging(contactSpecParams.PageSize * (contactSpecParams.PageIndex - 1), contactSpecParams.PageSize);
    }

    public ContactSpecification(Guid id) : base(x => x.Id == id)
    {
    }
}