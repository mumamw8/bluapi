using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.EstimateDtos;

public class EstimateAddDto
{
    [Required]
    public string? EstimateNumber { get; set; }
    public string? Description { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    // FK
    public Guid ClientId { get; set; }
    public Guid WorkspaceId { get; set; }
    public int EstimateStatusId { get; set; }
}