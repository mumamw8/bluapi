using System.Linq.Expressions;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;

namespace Core.Specifications.WorkspaceSpecifications;

public class WorkspaceWithFiltersForCount : BaseSpecification<Workspace>
{
    public WorkspaceWithFiltersForCount(WorkspaceSpecParams workspaceSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(workspaceSpecParams.Search) || x.Name!.ToLower().Contains(workspaceSpecParams.Search)) &&
            (string.IsNullOrEmpty(workspaceSpecParams.AppUserId) || x.AppUserId == workspaceSpecParams.AppUserId)
        )
    {
    }
}