using System.Text.Json.Serialization;

namespace LabRegistry.Client.Models;

public class InspectionObjectModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [JsonPropertyName("productType")]
    public string ProductType { get; set; } = string.Empty;

    [JsonPropertyName("receiptDate")]
    public DateTimeOffset ReceiptDate { get; set; }

    [JsonPropertyName("productResult")]
    public string ProductResult { get; set; } = string.Empty;

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}