namespace LabRegistry.Application.InspectionObjects.CreateInspectionObject;

public class CreateInspectionObjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public DateTimeOffset RecieptDate { get; set; }
    public string? Comment { get; set; }
}
