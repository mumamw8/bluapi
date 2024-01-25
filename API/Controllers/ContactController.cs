using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.ContactDtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ContactController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<ContactReturnDto>>> GetContacts([FromQuery]ContactSpecParams contactSpecParams)
    {
        var spec = new ContactSpecification(contactSpecParams); // specs to evaluate
        var countSpec = new ContactWithFiltersForCount(contactSpecParams); // count spec
        
        var contacts = await _unitOfWork.Repository<Contact>()!.ListAsync(spec);
        // if (contacts == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Contact>()!.CountAsync(countSpec);

        var data = contacts.Select(contact => new ContactReturnDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber
        }).ToList();

        return Ok(new Pagination<ContactReturnDto>(contactSpecParams.PageIndex, contactSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactReturnDto>> GetContact(Guid id)
    {
        var spec = new ContactSpecification(id);
        var contact = await _unitOfWork.Repository<Contact>()!.GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new ContactReturnDto
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            PhoneNumber = contact.PhoneNumber
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(ContactAddDto contactAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(new WorkspaceUserMappingSpecParams { WorkspaceId = contactAddDto.WorkspaceId, AppUserId = currentUserId});
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);
        
        var contact = new Contact
        {
            FirstName = contactAddDto.FirstName,
            LastName = contactAddDto.LastName,
            Email = contactAddDto.Email,
            PhoneNumber = contactAddDto.PhoneNumber,
            WorkspaceId = contactAddDto.WorkspaceId
        };
        if (mapping.Count > 0) _unitOfWork.Repository<Contact>()!.Add(contact);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating contact"));
        return Ok(contact);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Contact>> UpdateContact(Contact contactToUpdate)
    {
        // only updates values sent
        _unitOfWork.Repository<Contact>()!.Update(contactToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating contact"));
        return Ok(contactToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteContact(Guid id)
    {
        var contact = await _unitOfWork.Repository<Contact>()!.GetByIdAsync(id);
        _unitOfWork.Repository<Contact>()!.Delete(contact);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting contact"));
        return Ok();
    }
}