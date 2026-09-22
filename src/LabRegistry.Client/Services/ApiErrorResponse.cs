using System.Text.Json.Serialization;

namespace LabRegistry.Client.Services;

public class ApiErrorResponse
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
}