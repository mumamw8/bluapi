using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos.EstimateDtos;

public class EstimateReturnDto
{
    public Guid Id { get; set; }
    public string? EstimateNumber { get; set; }
    public string? Description { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    // includes
    public virtual Client? Client { get; set; }
    public virtual EstimateStatus? Status { get; set; }
    // FK
    public Guid ClientId { get; set; }
    public int EstimateStatusId { get; set; }
    public Guid WorkspaceId { get; set; }
}