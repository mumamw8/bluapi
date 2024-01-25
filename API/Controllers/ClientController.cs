using System.Security.Claims;
using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.ClientDtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<ClientReturnDto>>> GetClients([FromQuery]ClientSpecParams clientSpecParams)
    {
        var spec = new ClientWithWorkspaceAndContactSpecification(clientSpecParams); // specs to evaluate
        var countSpec = new ClientWithFiltersForCount(clientSpecParams); // count spec
        
        var clients = await _unitOfWork.Repository<Client>()!.ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Client>()!.CountAsync(countSpec);
        
        var data = clients.Select(client => new ClientReturnDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Address = client.Address,
            PhoneNumber = client.PhoneNumber,
            ContactId = client.ContactId,
            WorkspaceId = client.WorkspaceId,
            ProfileImageUrl = client.ProfileImageUrl
        }).ToList();

        return Ok(new Pagination<ClientReturnDto>(clientSpecParams.PageIndex, clientSpecParams.PageSize, totalItems, data));
    }

    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientReturnDto>> GetClient(Guid id)
    {
        var spec = new ClientWithWorkspaceAndContactSpecification(id);
        var client = await _unitOfWork.Repository<Client>().GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new ClientReturnDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Address = client.Address,
            PhoneNumber = client.PhoneNumber,
            ContactId = client.ContactId,
            WorkspaceId = client.WorkspaceId,
            Workspace = client.Workspace,
            Contact = client.Contact,
            ProfileImageUrl = client.ProfileImageUrl
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(ClientAddDto clientAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(new WorkspaceUserMappingSpecParams { WorkspaceId = clientAddDto.WorkspaceId, AppUserId = currentUserId});
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);

        var client = new Client
        {
            Name = clientAddDto.Name,
            Email = clientAddDto.Email,
            Address = clientAddDto.Address,
            PhoneNumber = clientAddDto.PhoneNumber,
            ContactId = clientAddDto.ContactId,
            WorkspaceId = clientAddDto.WorkspaceId,
            ProfileImageUrl = clientAddDto.ProfileImageUrl
        };
        
        if (mapping.Count > 0)
        {
            _unitOfWork.Repository<Client>()!.Add(client);
        }
        
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating client"));
        return Ok(client);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Client>> UpdateClient(Client clientToUpdate)
    {
        _unitOfWork.Repository<Client>()!.Update(clientToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating client"));
        return Ok(clientToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteClient(Guid id)
    {
        var client = await _unitOfWork.Repository<Client>()!.GetByIdAsync(id);
        _unitOfWork.Repository<Client>()!.Delete(client);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting client"));
        return Ok();
    }
    
}