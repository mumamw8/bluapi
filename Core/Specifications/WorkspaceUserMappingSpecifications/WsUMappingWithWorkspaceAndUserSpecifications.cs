using System.Linq.Expressions;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;

namespace Core.Specifications.WorkspaceUserMappingSpecifications;

public class WsUMappingWithWorkspaceAndUserSpecifications : BaseSpecification<WorkspaceUserMapping>
{
    public WsUMappingWithWorkspaceAndUserSpecifications(WorkspaceUserMappingSpecParams workspaceUserMappingSpecParams) 
        : base(x =>
            (!workspaceUserMappingSpecParams.WorkspaceId.HasValue || x.WorkspaceId == workspaceUserMappingSpecParams.WorkspaceId) &&
            (string.IsNullOrEmpty(workspaceUserMappingSpecParams.AppUserId) || x.AppUserId == workspaceUserMappingSpecParams.AppUserId)
        )
    {
        // AddInclude(x => x.Workspace!);
        // AddInclude(x => x.AppUser!);
        // AddOrderBy(x => x.AppUser!.LastName!);
        ApplyPaging(workspaceUserMappingSpecParams.PageSize * (workspaceUserMappingSpecParams.PageIndex - 1), workspaceUserMappingSpecParams.PageSize);
    }
    
    public WsUMappingWithWorkspaceAndUserSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.Workspace!);
        AddInclude(x => x.AppUser!);
    }
}