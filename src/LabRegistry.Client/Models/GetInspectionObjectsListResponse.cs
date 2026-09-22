using System.Text.Json.Serialization;

namespace LabRegistry.Client.Models;

public class GetInspectionObjectsListResponse
{
    [JsonPropertyName("inspectionObjects")]
    public List<InspectionObjectModel> InspectionObjects { get; set; } = [];

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}