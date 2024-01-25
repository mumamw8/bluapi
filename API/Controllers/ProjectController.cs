using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.ProjectDtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ProjectSpecifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<ProjectReturnDto>>> GetProjects([FromQuery]ProjectSpecParams projectSpecParams)
    {
        var spec = new ProjectWithClientAndStatusSpecifications(projectSpecParams); // specs to evaluate
        var countSpec = new ProjectWithFiltersForCount(projectSpecParams); // count spec
        
        var projects = await _unitOfWork.Repository<Project>()!.ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Project>()!.CountAsync(countSpec);
        
        var data = projects.Select(project => new ProjectReturnDto
        {
            Name = project.Name,
            CreatedAt = project.CreatedAt,
            Status = project.Status,
            Start = project.Start,
            End = project.End,
            Client = project.Client,
            ClientId = project.ClientId
        }).ToList();

        return Ok(new Pagination<ProjectReturnDto>(projectSpecParams.PageIndex, projectSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectReturnDto>> GetProject(Guid id)
    {
        var spec = new ProjectWithClientAndStatusSpecifications(id);
        var project = await _unitOfWork.Repository<Project>()!.GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new ProjectReturnDto
        {
            Name = project.Name,
            CreatedAt = project.CreatedAt,
            Status = project.Status,
            Start = project.Start,
            End = project.End,
            Client = project.Client,
            ClientId = project.ClientId
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(ProjectAddDto projectAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(new WorkspaceUserMappingSpecParams { WorkspaceId = projectAddDto.WorkspaceId, AppUserId = currentUserId});
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);
        
        var project = new Project
        {
            Name = projectAddDto.Name,
            ProjectStatusId = projectAddDto.ProjectStatusId,
            Start = projectAddDto.Start,
            End = projectAddDto.End,
            ClientId = projectAddDto.ClientId,
            WorkspaceId = projectAddDto.WorkspaceId
        };
        if (mapping.Count > 0) _unitOfWork.Repository<Project>()?.Add(project);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating project"));
        return Ok(project);
    }
    
    //update
    [HttpPut]
    public async Task<ActionResult<Project>> UpdateProject(Project projectToUpdate)
    {
        _unitOfWork.Repository<Project>()?.Update(projectToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating project"));
        return Ok(projectToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        var project = await _unitOfWork.Repository<Project>()?.GetByIdAsync(id)!;
        _unitOfWork.Repository<Project>()?.Delete(project);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting project"));
        return Ok();
    }
    
    // get statuses
    [HttpGet("statuses")]
    public async Task<ActionResult<IReadOnlyList<ProjectStatus>>> GetProjectStatuses()
    {
        var statuses = await _unitOfWork.Repository<ProjectStatus>()?.ListAllAsync()!;
        // if (statuses.Count < 0) return NotFound(new ApiResponse(404));
        return Ok(statuses);
    }
}