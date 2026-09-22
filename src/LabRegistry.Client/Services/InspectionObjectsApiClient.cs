using LabRegistry.Client.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LabRegistry.Client.Services;

public class InspectionObjectsApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<GetInspectionObjectsListResponse> GetListAsync(
        string? namePart,
        string? productType,
        string? productResult,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var parameters = new List<string>
        {
            $"Page={page}",
            $"PageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(namePart))
        {
            parameters.Add($"NamePart={Uri.EscapeDataString(namePart)}");
        }

        if (!string.IsNullOrWhiteSpace(productType))
        {
            parameters.Add($"ProductType={Uri.EscapeDataString(productType)}");
        }

        if (!string.IsNullOrWhiteSpace(productResult))
        {
            parameters.Add($"ProductResult={Uri.EscapeDataString(productResult)}");
        }

        var url = "api/inspection-objects";
        url += "?" + string.Join("&", parameters);

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken);

        await EnsureSuccessAsync(response);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        var result =
            JsonSerializer.Deserialize<GetInspectionObjectsListResponse>(
                json,
                JsonOptions);

        return result ?? new GetInspectionObjectsListResponse();
    }

    public async Task<InspectionObjectModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"api/inspection-objects/{id}",
            cancellationToken);

        await EnsureSuccessAsync(response);

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<InspectionObjectModel>(
            json,
            JsonOptions);
    }

    public async Task CreateAsync(
        CreateInspectionObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(
            request,
            JsonOptions);

        using var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        using var response = await _httpClient.PostAsync(
            "api/inspection-objects",
            content,
            cancellationToken);

        await EnsureSuccessAsync(response);
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateInspectionObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(
            request,
            JsonOptions);

        using var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Patch,
            $"api/inspection-objects/{id}")
        {
            Content = content
        };

        using var response = await _httpClient.SendAsync(
            requestMessage,
            cancellationToken);

        await EnsureSuccessAsync(response);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await ExtractErrorMessageAsync(response);

        throw new ApiException(response.StatusCode, message);
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var json = await response.Content.ReadAsStringAsync();

            var errors =
                JsonSerializer.Deserialize<List<ApiErrorResponse>>(
                    json,
                    JsonOptions);

            if (errors is not null && errors.Count > 0)
            {
                return string.Join(
                    Environment.NewLine,
                    errors
                        .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                        .Select(x => x.ErrorMessage));
            }
        }
        catch (JsonException)
        {
            // Если ответ сервера не соответствует ожидаемому формату ошибки,
            // используем сообщение ниже.
        }

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound =>
                "Объект не найден.",

            HttpStatusCode.BadRequest =>
                "Некорректные данные запроса.",

            HttpStatusCode.Conflict =>
                "Конфликт данных.",

            HttpStatusCode.ServiceUnavailable =>
                "Сервис временно недоступен.",

            _ =>
                $"Сервер вернул ошибку {(int)response.StatusCode}."
        };
    }
}
