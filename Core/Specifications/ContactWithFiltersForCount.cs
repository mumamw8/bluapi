using System.Linq.Expressions;
using Core.Dtos.ContactDtos;
using Core.Entities;

namespace Core.Specifications;

public class ContactWithFiltersForCount : BaseSpecification<Contact>
{
    public ContactWithFiltersForCount(ContactSpecParams contactSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(contactSpecParams.Search) || x.FirstName!.ToLower().Contains(contactSpecParams.Search)) &&
            (string.IsNullOrEmpty(contactSpecParams.Search) || x.LastName!.ToLower().Contains(contactSpecParams.Search))
        )
    {
    }
}