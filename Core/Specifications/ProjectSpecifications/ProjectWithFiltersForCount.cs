using Core.Dtos.ProjectDtos;
using Core.Entities;

namespace Core.Specifications.ProjectSpecifications;

public class ProjectWithFiltersForCount : BaseSpecification<Project>
{
    public ProjectWithFiltersForCount(ProjectSpecParams projectSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(projectSpecParams.Search) || x.Name!.ToLower().Contains(projectSpecParams.Search)) &&
            (!projectSpecParams.ClientId.HasValue || x.ClientId == projectSpecParams.ClientId) &&
            (!projectSpecParams.WorkspaceId.HasValue || x.WorkspaceId == projectSpecParams.WorkspaceId) &&
            (!projectSpecParams.ProjectStatusId.HasValue || x.ProjectStatusId == projectSpecParams.ProjectStatusId)
        )
    {
    }
}