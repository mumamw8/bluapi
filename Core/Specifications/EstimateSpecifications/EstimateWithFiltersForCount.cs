using Core.Dtos.EstimateDtos;
using Core.Entities;

namespace Core.Specifications.EstimateSpecifications;

public class EstimateWithFiltersForCount : BaseSpecification<Estimate>
{
    public EstimateWithFiltersForCount(EstimateSpecParams estimateSpecParams) 
        : base(x =>
            (string.IsNullOrEmpty(estimateSpecParams.Search) || x.EstimateNumber!.ToLower().Contains(estimateSpecParams.Search)) &&
            (!estimateSpecParams.ClientId.HasValue || x.ClientId == estimateSpecParams.ClientId) &&
            (!estimateSpecParams.EstimateStatusId.HasValue || x.EstimateStatusId == estimateSpecParams.EstimateStatusId)
        )
    {
    }
}