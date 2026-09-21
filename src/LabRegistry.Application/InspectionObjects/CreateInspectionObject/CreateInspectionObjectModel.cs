namespace LabRegistry.Application.InspectionObjects.CreateInspectionObject;

public record CreateInspectionObjectModel(
    Guid Id,
    string Name,
    string Version,
    string ProductType,
    DateTimeOffset RecieptDate,
    string ProductResult,
    string? Comment
);
