using System.Linq.Expressions;
using Core.Dtos.TimeRecordDtos;
using Core.Entities;

namespace Core.Specifications.TimeRecordSpecifications;

public class TimeRecordWithProjectAndUserSpecifications : BaseSpecification<TimeRecord>
{
    public TimeRecordWithProjectAndUserSpecifications(TimeRecordSpecParams timeRecordSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(timeRecordSpecParams.Search) || x.Description!.ToLower().Contains(timeRecordSpecParams.Search)) &&
            (!timeRecordSpecParams.ProjectId.HasValue || x.ProjectId == timeRecordSpecParams.ProjectId) &&
            (!timeRecordSpecParams.WorkspaceId.HasValue || x.WorkspaceId == timeRecordSpecParams.WorkspaceId) &&
            (string.IsNullOrEmpty(timeRecordSpecParams.AppUserId) || x.AppUserId == timeRecordSpecParams.AppUserId)
        )
    {
        AddInclude(x => x.Project!);
        // AddInclude(x => x.AppUser!);
        AddOrderBy(x => x.Start);
        ApplyPaging(timeRecordSpecParams.PageSize * (timeRecordSpecParams.PageIndex - 1), timeRecordSpecParams.PageSize);
    }
    
    public TimeRecordWithProjectAndUserSpecifications(Guid id) 
        : base(x =>
            x.Id == id
        )
    {
        AddInclude(x => x.Project!);
        AddInclude(x => x.AppUser!);
    }
}