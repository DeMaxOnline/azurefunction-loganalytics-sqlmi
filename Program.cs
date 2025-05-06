using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Monitor.Query;
using System.Text.Json;
using TobaHR.Models;
using TobaHR.Services;
using Azure.Core;

var host = Host.CreateDefaultBuilder()
    .ConfigureFunctionsWebApplication() // <-- NEW: ASP.NET Core integration
    .ConfigureServices(services =>
    {
        services.AddLogging();

        _ = services.AddSingleton<LogAnalyticsSettings>(provider =>
        {
            var json = Environment.GetEnvironmentVariable("LogAnalyticsConfig");
            return JsonSerializer.Deserialize<LogAnalyticsSettings>(json);
        });

        services.AddSingleton(new LogsQueryClient(new DefaultAzureCredential()));
        services.AddSingleton<TokenCredential>(new DefaultAzureCredential());
        services.AddSingleton<ILogAnalyticsService, LogAnalyticsService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
    })
    .Build();

host.Run();
