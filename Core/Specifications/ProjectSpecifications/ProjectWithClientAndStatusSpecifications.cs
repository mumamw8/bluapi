using System.Linq.Expressions;
using Core.Dtos.ProjectDtos;
using Core.Entities;

namespace Core.Specifications.ProjectSpecifications;

public class ProjectWithClientAndStatusSpecifications : BaseSpecification<Project>
{
    public ProjectWithClientAndStatusSpecifications(ProjectSpecParams projectSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(projectSpecParams.Search) || x.Name!.ToLower().Contains(projectSpecParams.Search)) &&
            (!projectSpecParams.ClientId.HasValue || x.ClientId == projectSpecParams.ClientId) &&
            (!projectSpecParams.WorkspaceId.HasValue || x.WorkspaceId == projectSpecParams.WorkspaceId) &&
            (!projectSpecParams.ProjectStatusId.HasValue || x.ProjectStatusId == projectSpecParams.ProjectStatusId)
        )
    {
        AddInclude(x => x.Client!);
        AddInclude(x => x.Status!);
        AddOrderBy(x => x.Name!);
        ApplyPaging(projectSpecParams.PageSize * (projectSpecParams.PageIndex - 1), projectSpecParams.PageSize);
    }
    
    public ProjectWithClientAndStatusSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.Client!);
        AddInclude(x => x.Status!);
    }
}