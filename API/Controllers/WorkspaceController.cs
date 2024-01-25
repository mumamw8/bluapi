using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.WorkspaceSpecifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkspaceController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<WorkspaceReturnDto>>> GetWorkspaces([FromQuery]WorkspaceSpecParams workspaceSpecParams)
    {
        // TODO: return workspace without workspaceusermappings
        
        var spec = new WorkspaceWithUserAndWsUMappingsSpecifications(workspaceSpecParams); // specs to evaluate
        var countSpec = new WorkspaceWithFiltersForCount(workspaceSpecParams); // count spec
        
        var workspaces = await _unitOfWork.Repository<Workspace>()!.ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Workspace>()!.CountAsync(countSpec);
        
        var data = workspaces.Select(workspace => new WorkspaceReturnDto
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            ImageUrl = workspace.ImageUrl,
            AppUserId = workspace.AppUserId
        }).ToList();

        return Ok(new Pagination<WorkspaceReturnDto>(workspaceSpecParams.PageIndex, workspaceSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkspaceReturnDto>> GetWorkspace(Guid id)
    {
        var spec = new WorkspaceWithUserAndWsUMappingsSpecifications(id);
        var workspace = await _unitOfWork.Repository<Workspace>()!.GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new WorkspaceReturnDto
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            ImageUrl = workspace.ImageUrl,
            AppUserId = workspace.AppUserId
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Workspace>> CreateWorkspace(WorkspaceAddDto workspaceAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var workspace = new Workspace
        {
            Name = workspaceAddDto.Name,
            Description = workspaceAddDto.Description,
            ImageUrl = workspaceAddDto.ImageUrl,
            AppUserId = currentUserId
        };
        _unitOfWork.Repository<Workspace>()!.Add(workspace);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating workspace"));
        return Ok(workspace);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Workspace>> UpdateWorkspace(Workspace workspaceToUpdate)
    {
        // only updates values sent
        _unitOfWork.Repository<Workspace>()!.Update(workspaceToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating workspace"));
        return Ok(workspaceToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteWorkspace(Guid id)
    {
        var workspace = await _unitOfWork.Repository<Workspace>()!.GetByIdAsync(id);
        _unitOfWork.Repository<Workspace>()!.Delete(workspace);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting workspace"));
        return Ok();
    }
    
    // get list WorkspaceMappings
    [HttpGet("mapping")]
    public async Task<ActionResult<Pagination<WorkspaceUserMapping>>> GetMembers([FromQuery]WorkspaceUserMappingSpecParams workspaceUserMappingSpecParams)
    {
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(workspaceUserMappingSpecParams);
        var countSpec = new WorkspaceUserMappingWithFiltersForCount(workspaceUserMappingSpecParams);
        
        var mappings = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);
        var totalItems = await _unitOfWork.Repository<WorkspaceUserMapping>()!.CountAsync(countSpec);
        // if (statuses.Count < 0) return NotFound(new ApiResponse(404));
        return Ok(new Pagination<WorkspaceUserMapping>(workspaceUserMappingSpecParams.PageIndex, workspaceUserMappingSpecParams.PageSize, totalItems, mappings));
    }
    
    // get WorkSpaceMapping
    [HttpGet("mapping/{id:guid}")]
    public async Task<ActionResult<WorkspaceUserMapping>> GetMember(Guid id)
    {
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(id);
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.GetEntityWithSpec(spec);
        return mapping;
    }
    
    // create WorkspaceUserMapping
    [HttpPost("mapping")]
    public async Task<ActionResult<Contact>> CreateWorkspaceUserMapping(WorkspaceUserMappingAddDto workspaceUserMappingAddDto)
    {
        var workspace = await _unitOfWork.Repository<Workspace>()!.GetByIdAsync(workspaceUserMappingAddDto.WorkspaceId);
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();

        if (workspace.AppUserId != currentUserId)
            return Unauthorized("No permission to perform this action");
        
        var workspaceUserMapping = new WorkspaceUserMapping
        {
            WorkspaceId = workspaceUserMappingAddDto.WorkspaceId,
            AppUserId = workspaceUserMappingAddDto.AppUserId
        };
        
        _unitOfWork.Repository<WorkspaceUserMapping>()!.Add(workspaceUserMapping);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating workspace mapping"));
        return Ok(workspaceUserMapping);
    }
    
    // update WorkspaceUserMapping
    [HttpPut("mapping")]
    public async Task<ActionResult<WorkspaceUserMapping>> UpdateWorkspaceUserMapping(WorkspaceUserMapping workspaceUserMappingToUpdate)
    {
        // TODO: Check if workspace owner is same as current user
        var workspace = await _unitOfWork.Repository<Workspace>()!.GetByIdAsync(workspaceUserMappingToUpdate.WorkspaceId);
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();

        if (workspace.AppUserId != currentUserId)
            return Unauthorized("No permission to perform this action");

        // only updates values sent
        _unitOfWork.Repository<WorkspaceUserMapping>()!.Update(workspaceUserMappingToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating workspace user mapping"));
        return Ok(workspaceUserMappingToUpdate);
    }
    
    // delete WorkspaceUserMapping
    [HttpDelete("mapping/{id:guid}")]
    public async Task<ActionResult> DeleteWorkspaceUserMapping(Guid id)
    {
        var workspaceUserMapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.GetByIdAsync(id);
        _unitOfWork.Repository<WorkspaceUserMapping>()!.Delete(workspaceUserMapping);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting workspace user mapping"));
        return Ok();
    }
}