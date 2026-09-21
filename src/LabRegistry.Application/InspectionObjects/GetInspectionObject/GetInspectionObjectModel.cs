namespace LabRegistry.Application.InspectionObjects.GetInspectionObject;

public record GetInspectionObjectModel(Guid Id, string Name, string Version, string ProductType, DateTimeOffset ReceiptDate, string ProductResult, string? Comment);
