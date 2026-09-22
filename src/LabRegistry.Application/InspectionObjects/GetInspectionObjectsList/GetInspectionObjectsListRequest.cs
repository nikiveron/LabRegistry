namespace LabRegistry.Application.InspectionObjects.GetInspectionObjectsList;

public class GetInspectionObjectsListRequest
{
    public string? NamePart { get; set; }
    public string? ProductType { get; set; }
    public string? ProductResult { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
