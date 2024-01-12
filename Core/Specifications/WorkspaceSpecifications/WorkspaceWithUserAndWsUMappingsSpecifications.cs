using System.Linq.Expressions;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;

namespace Core.Specifications.WorkspaceSpecifications;

public class WorkspaceWithUserAndWsUMappingsSpecifications : BaseSpecification<Workspace>
{
    public WorkspaceWithUserAndWsUMappingsSpecifications(WorkspaceSpecParams workspaceSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(workspaceSpecParams.Search) || x.Name!.ToLower().Contains(workspaceSpecParams.Search)) &&
            (string.IsNullOrEmpty(workspaceSpecParams.AppUserId) || x.AppUserId == workspaceSpecParams.AppUserId)
        )
    {
        AddInclude(x => x.AppUser!);
        AddInclude(x => x.WorkspaceUserMappings!);
        AddOrderBy(x => x.Name!);
        ApplyPaging(workspaceSpecParams.PageSize * (workspaceSpecParams.PageIndex - 1), workspaceSpecParams.PageSize);
    }
    
    public WorkspaceWithUserAndWsUMappingsSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.AppUser!);
        AddInclude(x => x.WorkspaceUserMappings!);
    }
}