using LabRegistry.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LabRegistry.Domain.Entities;

public class InspectionObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(200)]
    public required string Name { get; set; }
    [MaxLength(50)]
    public required string Version { get; set; }
    public ProductType ProductType { get; set; }
    public DateTimeOffset ReceiptDate { get; set; }
    public ProductResult ProductResult { get; set; } = ProductResult.InProgress;
    [MaxLength(1000)]
    public string? Comment { get; set; } = null;
}
