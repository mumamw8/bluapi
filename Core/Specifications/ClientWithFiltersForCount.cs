using System.Linq.Expressions;
using Core.Dtos.ClientDtos;
using Core.Entities;

namespace Core.Specifications;

public class ClientWithFiltersForCount : BaseSpecification<Client>
{
    public ClientWithFiltersForCount(ClientSpecParams clientSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(clientSpecParams.Search) || x.Name!.ToLower().Contains(clientSpecParams.Search)) &&
            (!clientSpecParams.WorkspaceId.HasValue || x.WorkspaceId == clientSpecParams.WorkspaceId) &&
            (!clientSpecParams.ContactId.HasValue || x.ContactId == clientSpecParams.ContactId)
        )
    {
    }
}