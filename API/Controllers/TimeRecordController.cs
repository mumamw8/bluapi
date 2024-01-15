using API.Errors;
using Core.Dtos;
using Core.Dtos.TimeRecordDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.TimeRecordSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeRecordController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public TimeRecordController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<TimeRecordReturnDto>>> GetTimeRecords([FromQuery]TimeRecordSpecParams timeRecordSpecParams)
    {
        var spec = new TimeRecordWithProjectAndUserSpecifications(timeRecordSpecParams); // specs to evaluate
        var countSpec = new TimeRecordWithFiltersForCount(timeRecordSpecParams); // count spec
        
        var timeRecords = await _unitOfWork.Repository<TimeRecord>().ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<TimeRecord>().CountAsync(countSpec);
        
        var data = timeRecords.Select(timeRecord => new TimeRecordReturnDto
        {
            Id = timeRecord.Id,
            Start = timeRecord.Start,
            End = timeRecord.End,
            Duration = timeRecord.Duration,
            Description = timeRecord.Description,
            ProjectId = timeRecord.Id,
            AppUserId = timeRecord.AppUserId,
            Project = timeRecord.Project,
            AppUser = timeRecord.AppUser
        }).ToList();

        return Ok(new Pagination<TimeRecordReturnDto>(timeRecordSpecParams.PageIndex, timeRecordSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TimeRecordReturnDto>> GetTimeRecord(Guid id)
    {
        var spec = new TimeRecordWithProjectAndUserSpecifications(id);
        var timeRecord = await _unitOfWork.Repository<TimeRecord>().GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new TimeRecordReturnDto
        {
            Id = timeRecord.Id,
            Start = timeRecord.Start,
            End = timeRecord.End,
            Duration = timeRecord.Duration,
            Description = timeRecord.Description,
            ProjectId = timeRecord.ProjectId,
            AppUserId = timeRecord.AppUserId,
            Project = timeRecord.Project,
            AppUser = timeRecord.AppUser
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<TimeRecord>> CreateTimeRecord(TimeRecordAddDto timeRecordAddDto)
    {
        var timeRecord = new TimeRecord
        {
            Start = timeRecordAddDto.Start,
            End = timeRecordAddDto.End,
            Duration = timeRecordAddDto.Duration,
            Description = timeRecordAddDto.Description,
            ProjectId = timeRecordAddDto.ProjectId,
            AppUserId = ""
        };
        _unitOfWork.Repository<TimeRecord>().Add(timeRecord);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating time record"));
        return Ok(timeRecord);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<TimeRecord>> UpdateTimeRecord(TimeRecord timeRecordToUpdate)
    {
        _unitOfWork.Repository<TimeRecord>().Update(timeRecordToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating timeRecord"));
        return Ok(timeRecordToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTimeRecord(Guid id)
    {
        var timeRecord = await _unitOfWork.Repository<TimeRecord>().GetByIdAsync(id);
        _unitOfWork.Repository<TimeRecord>().Delete(timeRecord);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting time record"));
        return Ok();
    }
}