using System.Linq.Expressions;
using Core.Dtos.TimeRecordDtos;
using Core.Entities;

namespace Core.Specifications.TimeRecordSpecifications;

public class TimeRecordWithFiltersForCount : BaseSpecification<TimeRecord>
{
    public TimeRecordWithFiltersForCount(TimeRecordSpecParams timeRecordSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(timeRecordSpecParams.Search) || x.Description!.ToLower().Contains(timeRecordSpecParams.Search)) &&
            (!timeRecordSpecParams.ProjectId.HasValue || x.ProjectId == timeRecordSpecParams.ProjectId) &&
            (!timeRecordSpecParams.WorkspaceId.HasValue || x.WorkspaceId == timeRecordSpecParams.WorkspaceId) &&
            (string.IsNullOrEmpty(timeRecordSpecParams.AppUserId) || x.AppUserId == timeRecordSpecParams.AppUserId)
        )
    {
    }
}