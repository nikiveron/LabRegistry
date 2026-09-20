using System.ComponentModel.DataAnnotations;

namespace LabRegistry.Domain.Entities;

public class InspectionObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(AppConstants.InspectionObjectNameLength)]
    public required string Name { get; set; }
    [MaxLength(AppConstants.InspectionObjectVersionLength)]
    public required string Version { get; set; }
    public required string ProductType { get; set; }
    public DateTimeOffset ReceiptDate { get; set; }
    public required string ProductResult { get; set; }
    [MaxLength(AppConstants.InspectionObjectCommentLength)]
    public string? Comment { get; set; } = null;
}
