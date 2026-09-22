using System.Text.Json.Serialization;

namespace LabRegistry.Client.Models;

public class CreateInspectionObjectRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("productType")]
    public string ProductType { get; set; } = string.Empty;

    [JsonPropertyName("recieptDate")]
    public DateTimeOffset RecieptDate { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}