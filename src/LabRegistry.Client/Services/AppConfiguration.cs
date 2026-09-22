using Microsoft.Extensions.Configuration;

namespace LabRegistry.Client.Services;

public static class AppConfiguration
{
    private static readonly IConfiguration Configuration =
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    public static string ApiBaseUrl =>
        Configuration["Api:BaseUrl"]
        ?? throw new InvalidOperationException(
            "API BaseUrl is not configured.");
}