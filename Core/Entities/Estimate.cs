using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.Entities;

public class Estimate : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string? EstimateNumber { get; set; }
    public string? Description { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // nav props
    public virtual Client? Client { get; set; }
    public virtual EstimateStatus? Status { get; set; }
    // FK
    [Required]
    public Guid ClientId { get; set; }
    [Required]
    public int EstimateStatusId { get; set; }
}

public class EstimateStatus
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    
    // nav props
    // public virtual ICollection<Estimate>? Estimates { get; set; }
}