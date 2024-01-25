using API.Errors;
using API.Extensions;
using Core.Dtos;
using Core.Dtos.ReceiptDtos;
using Core.Dtos.WorkspaceDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ReceiptSpecifications;
using Core.Specifications.WorkspaceUserMappingSpecifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ReceiptController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // get list
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pagination<ReceiptReturnDto>>> GetReceipts([FromQuery]ReceiptSpecParams receiptSpecParams)
    {
        var spec = new ReceiptWithInvoiceSpecifications(receiptSpecParams); // specs to evaluate
        var countSpec = new ReceiptWithFiltersForCount(receiptSpecParams); // count spec
        
        var receipts = await _unitOfWork.Repository<Receipt>()!.ListAsync(spec);
        // if (clients == null) return NotFound(new ApiResponse(404));
        var totalItems = await _unitOfWork.Repository<Receipt>()!.CountAsync(countSpec);
        
        var data = receipts.Select(receipt => new ReceiptReturnDto
        {
            Id = receipt.Id,
            ReceiptNumber = receipt.ReceiptNumber,
            ReceiptFileUrl = receipt.ReceiptFileUrl,
            InvoiceId = receipt.InvoiceId,
            Invoice = receipt.Invoice
        }).ToList();

        return Ok(new Pagination<ReceiptReturnDto>(receiptSpecParams.PageIndex, receiptSpecParams.PageSize, totalItems, data));
    }
    
    // get
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReceiptReturnDto>> GetReceipt(Guid id)
    {
        var spec = new ReceiptWithInvoiceSpecifications(id);
        var receipt = await _unitOfWork.Repository<Receipt>()!.GetEntityWithSpec(spec);
        // if (client == null) return NotFound(new ApiResponse(404));
        return new ReceiptReturnDto
        {
            Id = receipt.Id,
            ReceiptNumber = receipt.ReceiptNumber,
            ReceiptFileUrl = receipt.ReceiptFileUrl,
            InvoiceId = receipt.InvoiceId,
            Invoice = receipt.Invoice
        };
    }
    
    // create
    [HttpPost]
    public async Task<ActionResult<Receipt>> CreateReceipt(ReceiptAddDto receiptAddDto)
    {
        var currentUserId = HttpContext.User.RetrieveUserIdFromPrincipal();
        var spec = new WsUMappingWithWorkspaceAndUserSpecifications(new WorkspaceUserMappingSpecParams { WorkspaceId = receiptAddDto.WorkspaceId, AppUserId = currentUserId});
        var mapping = await _unitOfWork.Repository<WorkspaceUserMapping>()!.ListAsync(spec);
        
        var receipt = new Receipt
        {
            ReceiptNumber = receiptAddDto.ReceiptNumber,
            ReceiptFileUrl = receiptAddDto.ReceiptFileUrl,
            InvoiceId = receiptAddDto.InvoiceId,
            WorkspaceId = receiptAddDto.WorkspaceId
        };
        if (mapping.Count > 0) _unitOfWork.Repository<Receipt>()?.Add(receipt);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating receipt"));
        return Ok(receipt);
    }
    
    // update
    [HttpPut]
    public async Task<ActionResult<Receipt>> UpdateReceipt(Receipt receiptToUpdate)
    {
        _unitOfWork.Repository<Receipt>()?.Update(receiptToUpdate);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating receipt"));
        return Ok(receiptToUpdate);
    }
    
    // delete
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteClient(Guid id)
    {
        var client = await _unitOfWork.Repository<Client>()!.GetByIdAsync(id);
        _unitOfWork.Repository<Client>()?.Delete(client);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting receipt"));
        return Ok();
    }
}