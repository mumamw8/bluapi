using API.Errors;
using Core.Dtos;
using Core.Dtos.EstimateDtos;
using Core.Entities;
using Core.Specifications.EstimateSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstimateController : Controller
{
    private readonly UnitOfWork _unitOfWork;

    public EstimateController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<EstimateReturnDto>>> GetEstimates([FromQuery]EstimateSpecParams estimateSpecParams)
    {
        var spec = new EstimateWithClientAndStatusSpecification(estimateSpecParams); // specs to evaluate
        var countSpec = new EstimateWithFiltersForCount(estimateSpecParams); // count spec
        
        var estimates = await _unitOfWork.Repository<Estimate>().ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Estimate>().CountAsync(countSpec);
        
        var data = estimates.Select(estimate => new EstimateReturnDto
        {
            Id = estimate.Id,
            EstimateNumber = estimate.EstimateNumber,
            Description = estimate.Description,
            SubTotal = estimate.SubTotal,
            Discount = estimate.Discount,
            Tax = estimate.Tax,
            Total = estimate.Total,
            ClientId = estimate.ClientId,
            EstimateStatusId = estimate.EstimateStatusId,
            Client = estimate.Client,
            Status = estimate.Status,
            CreatedAt = estimate.CreatedAt,
            UpdatedAt = estimate.UpdatedAt
        }).ToList();

        return Ok(new Pagination<EstimateReturnDto>(estimateSpecParams.PageIndex, estimateSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstimateReturnDto>> GetEstimate(Guid id)
    {
        var spec = new EstimateWithClientAndStatusSpecification(id);
        var estimate = await _unitOfWork.Repository<Estimate>().GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new EstimateReturnDto
        {
            Id = estimate.Id,
            EstimateNumber = estimate.EstimateNumber,
            Description = estimate.Description,
            SubTotal = estimate.SubTotal,
            Discount = estimate.Discount,
            Tax = estimate.Tax,
            Total = estimate.Total,
            ClientId = estimate.ClientId,
            EstimateStatusId = estimate.EstimateStatusId,
            Client = estimate.Client,
            Status = estimate.Status,
            CreatedAt = estimate.CreatedAt,
            UpdatedAt = estimate.UpdatedAt
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Estimate>> CreateEstimate(EstimateAddDto estimateAddDto)
    {
        var estimate = new Estimate
        {
            EstimateNumber = estimateAddDto.EstimateNumber,
            Description = estimateAddDto.Description,
            SubTotal = estimateAddDto.SubTotal,
            Discount = estimateAddDto.Discount,
            Tax = estimateAddDto.Tax,
            Total = estimateAddDto.Total,
            ClientId = estimateAddDto.ClientId,
            EstimateStatusId = estimateAddDto.EstimateStatusId
        };
        _unitOfWork.Repository<Estimate>().Add(estimate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating contact"));
        return Ok(estimate);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Estimate>> UpdateEstimate(Estimate estimateToUpdate)
    {
        _unitOfWork.Repository<Estimate>().Update(estimateToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating estimate"));
        return Ok(estimateToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteEstimate(Guid id)
    {
        var estimate = await _unitOfWork.Repository<Estimate>().GetByIdAsync(id);
        _unitOfWork.Repository<Estimate>().Delete(estimate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting estimate"));
        return Ok();
    }
    
    // get estimate statuses
    [HttpGet("statuses")]
    public async Task<ActionResult<List<EstimateStatus>>> GetEstimateStatuses()
    {
        var statuses = await _unitOfWork.Repository<EstimateStatus>().ListAllAsync();
        // if (statuses.Count < 0) return NotFound(new ApiResponse(404));
        return Ok(statuses);
    }
}