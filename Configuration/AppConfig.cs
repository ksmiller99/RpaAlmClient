using Microsoft.Extensions.Configuration;

namespace RpaAlmClient.Configuration;

public static class AppConfig
{
    private static IConfiguration? _configuration;

    public static string ApiBaseUrl
    {
        get
        {
            if (_configuration == null)
            {
                LoadConfiguration();
            }
            return _configuration?["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
        }
    }

    private static void LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }
}
