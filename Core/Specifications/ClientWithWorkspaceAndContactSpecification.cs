using System.Linq.Expressions;
using Core.Dtos.ClientDtos;
using Core.Entities;

namespace Core.Specifications;

public class ClientWithWorkspaceAndContactSpecification : BaseSpecification<Client>
{
    public ClientWithWorkspaceAndContactSpecification(ClientSpecParams clientSpecParams) 
        : base(x => 
            (string.IsNullOrEmpty(clientSpecParams.Search) || x.Name!.ToLower().Contains(clientSpecParams.Search)) &&
            (!clientSpecParams.WorkspaceId.HasValue || x.WorkspaceId == clientSpecParams.WorkspaceId) &&
            (!clientSpecParams.ContactId.HasValue || x.ContactId == clientSpecParams.ContactId)
        )
    {
        AddInclude(x => x.Workspace!);
        AddInclude(x => x.Contact!);
        AddOrderBy(x => x.Name!);
        ApplyPaging(clientSpecParams.PageSize * (clientSpecParams.PageIndex - 1), clientSpecParams.PageSize);
    }

    public ClientWithWorkspaceAndContactSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Workspace!);
        AddInclude(x => x.Contact!);
    }
}