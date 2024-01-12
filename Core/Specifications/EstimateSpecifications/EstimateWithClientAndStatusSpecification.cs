using System.Linq.Expressions;
using Core.Dtos.EstimateDtos;
using Core.Entities;

namespace Core.Specifications.EstimateSpecifications;

public class EstimateWithClientAndStatusSpecification : BaseSpecification<Estimate>
{
    public EstimateWithClientAndStatusSpecification(EstimateSpecParams estimateSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(estimateSpecParams.Search) || x.EstimateNumber!.ToLower().Contains(estimateSpecParams.Search)) &&
            (!estimateSpecParams.ClientId.HasValue || x.ClientId == estimateSpecParams.ClientId) &&
            (!estimateSpecParams.EstimateStatusId.HasValue || x.EstimateStatusId == estimateSpecParams.EstimateStatusId)
        )
    {
        AddInclude(x => x.Client!);
        AddInclude(x => x.Status!);
        AddOrderBy(x => x.EstimateNumber!);
        ApplyPaging(estimateSpecParams.PageSize * (estimateSpecParams.PageIndex - 1), estimateSpecParams.PageSize);
    }

    public EstimateWithClientAndStatusSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Client!);
        AddInclude(x => x.Status!);
    }
}