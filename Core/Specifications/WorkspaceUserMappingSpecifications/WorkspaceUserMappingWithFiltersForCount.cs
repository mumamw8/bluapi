using System.Linq.Expressions;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;

namespace Core.Specifications.WorkspaceUserMappingSpecifications;

public class WorkspaceUserMappingWithFiltersForCount : BaseSpecification<WorkspaceUserMapping>
{
    public WorkspaceUserMappingWithFiltersForCount(WorkspaceUserMappingSpecParams workspaceUserMappingSpecParams) 
        : base(x =>
            (!workspaceUserMappingSpecParams.WorkspaceId.HasValue || x.WorkspaceId == workspaceUserMappingSpecParams.WorkspaceId) &&
            (string.IsNullOrEmpty(workspaceUserMappingSpecParams.AppUserId) || x.AppUserId == workspaceUserMappingSpecParams.AppUserId)
        )
    {
    }
}