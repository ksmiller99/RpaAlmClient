using Microsoft.Extensions.Configuration;

namespace RpaAlmClient.Configuration;

public static class AppConfig
{
    private static IConfiguration? _configuration;
    private static readonly object _lock = new object();

    public static string ApiBaseUrl
    {
        get
        {
            if (_configuration == null)
            {
                lock (_lock)
                {
                    if (_configuration == null)
                    {
                        LoadConfiguration();
                    }
                }
            }

            // _configuration is guaranteed to be non-null after LoadConfiguration()
            var baseUrl = _configuration!["ApiSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException(
                    "API Base URL is not configured. Please ensure appsettings.json exists and contains ApiSettings:BaseUrl.");
            }

            return baseUrl;
        }
    }

    private static void LoadConfiguration()
    {
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = Path.Combine(basePath, "appsettings.json");

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException(
                $"Configuration file not found at: {configPath}. Please ensure appsettings.json is copied to the output directory.");
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
    }
}
