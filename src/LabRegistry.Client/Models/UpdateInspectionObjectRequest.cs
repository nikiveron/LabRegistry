using System.Text.Json.Serialization;

namespace LabRegistry.Client.Models;

public class UpdateInspectionObjectRequest
{
    [JsonPropertyName("productResult")]
    public string? ProductResult { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}