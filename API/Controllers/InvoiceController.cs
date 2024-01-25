using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.InvoiceDtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.InvoiceSpecifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<InvoiceReturnDto>>> GetInvoices([FromQuery]InvoiceSpecParams invoiceSpecParams)
    {
        var spec = new InvoiceWithProjectAndStatusSpecifications(invoiceSpecParams); // specs to evaluate
        var countSpec = new InvoiceWithFiltersForCount(invoiceSpecParams); // count spec
        
        var invoices = await _unitOfWork.Repository<Invoice>()?.ListAsync(spec)!;
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Invoice>()?.CountAsync(countSpec)!;
        
        var data = invoices.Select(invoice => new InvoiceReturnDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            From = invoice.From,
            To = invoice.To,
            SubTotal = invoice.SubTotal,
            Discount = invoice.Discount,
            Tax = invoice.Tax,
            Total = invoice.Total,
            Terms = invoice.Terms,
            DueDate = invoice.DueDate,
            InvoiceDate = invoice.InvoiceDate,
            ExtraNotes = invoice.ExtraNotes,
            ProjectId = invoice.ProjectId,
            InvoiceStatusId = invoice.InvoiceStatusId,
            Project = invoice.Project,
            Status = invoice.Status,
            LogoUrl = invoice.LogoUrl,
            InvoiceItems = invoice.InvoiceItems
        }).ToList();

        return Ok(new Pagination<InvoiceReturnDto>(invoiceSpecParams.PageIndex, invoiceSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InvoiceReturnDto>> GetInvoice(Guid id)
    {
        var spec = new InvoiceWithProjectAndStatusSpecifications(id);
        var invoice = await _unitOfWork.Repository<Invoice>()?.GetEntityWithSpec(spec)!;
        // if (client == null) return NotFound(new ApiResponse(404));
        return new InvoiceReturnDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            From = invoice.From,
            To = invoice.To,
            SubTotal = invoice.SubTotal,
            Discount = invoice.Discount,
            Tax = invoice.Tax,
            Total = invoice.Total,
            Terms = invoice.Terms,
            DueDate = invoice.DueDate,
            InvoiceDate = invoice.InvoiceDate,
            ExtraNotes = invoice.ExtraNotes,
            ProjectId = invoice.ProjectId,
            InvoiceStatusId = invoice.InvoiceStatusId,
            Project = invoice.Project,
            Status = invoice.Status,
            LogoUrl = invoice.LogoUrl,
            InvoiceItems = invoice.InvoiceItems
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Invoice>> CreateContact(InvoiceAddDto invoiceAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(new WorkspaceUserMappingSpecParams { WorkspaceId = invoiceAddDto.WorkspaceId, AppUserId = currentUserId});
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);
        
        var items = new List<InvoiceItem>();
        if (invoiceAddDto.InvoiceItems != null)
            items.AddRange(invoiceAddDto.InvoiceItems.Select(item => new InvoiceItem
            {
                Description = item.Description,
                Details = item.Description,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice,
                UnitPrice = item.UnitPrice
            }));

        var invoice = new Invoice
        {
            InvoiceNumber = invoiceAddDto.InvoiceNumber,
            From = invoiceAddDto.From,
            To = invoiceAddDto.To,
            SubTotal = invoiceAddDto.SubTotal,
            Discount = invoiceAddDto.Discount,
            Tax = invoiceAddDto.Tax,
            Total = invoiceAddDto.Total,
            Terms = invoiceAddDto.Terms,
            DueDate = invoiceAddDto.DueDate,
            InvoiceDate = invoiceAddDto.InvoiceDate,
            ExtraNotes = invoiceAddDto.ExtraNotes,
            ProjectId = invoiceAddDto.ProjectId,
            InvoiceStatusId = invoiceAddDto.InvoiceStatusId,
            LogoUrl = invoiceAddDto.LogoUrl,
            InvoiceItems = items,
            WorkspaceId = invoiceAddDto.WorkspaceId
        };
        if (mapping.Count > 0) _unitOfWork.Repository<Invoice>()?.Add(invoice);
        
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating invoice"));
        return Ok(invoice);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Invoice>> UpdateInvoice(Invoice invoiceToUpdate)
    {
        _unitOfWork.Repository<Invoice>()?.Update(invoiceToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating invoice"));
        return Ok(invoiceToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteInvoice(Guid id)
    {
        var invoice = await _unitOfWork.Repository<Invoice>()?.GetByIdAsync(id)!;
        _unitOfWork.Repository<Invoice>()?.Delete(invoice);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting invoice"));
        return Ok();
    }
    
    // get invoice statuses
    [HttpGet("statuses")]
    public async Task<ActionResult<List<InvoiceStatus>>> GetInvoiceStatuses()
    {
        var statuses = await _unitOfWork.Repository<InvoiceStatus>()?.ListAllAsync()!;
        // if (statuses.Count < 0) return NotFound(new ApiResponse(404));
        return Ok(statuses);
    }
    
    // get invoiceItems list
    [HttpGet("item")]
    public async Task<ActionResult<List<InvoiceItem>>> GetInvoiceItems()
    {
        var items = await _unitOfWork.Repository<InvoiceItem>()?.ListAllAsync()!;
        // if (statuses.Count < 0) return NotFound(new ApiResponse(404));
        return Ok(items);
    }
    
    // update invoiceItem
    [HttpPut("item")]
    public async Task<ActionResult<InvoiceItem>> UpdateInvoiceItem(InvoiceItem invoiceItemToUpdate)
    {
        _unitOfWork.Repository<InvoiceItem>()?.Update(invoiceItemToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating invoiceItem"));
        return Ok(invoiceItemToUpdate);
    }
    
    // delete invoiceItem
    [HttpDelete("item/{id:guid}")]
    public async Task<ActionResult> DeleteInvoiceItem(Guid id)
    {
        var invoiceItem = await _unitOfWork.Repository<InvoiceItem>()?.GetByIdAsync(id)!;
        _unitOfWork.Repository<InvoiceItem>()?.Delete(invoiceItem);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting invoiceItem"));
        return Ok();
    }
}