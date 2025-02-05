using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotionMovieUpdater;
using NotionMovieUpdater.Services;

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddConsole();
        });
        
        services.AddMovieService(configuration);
        services.AddNotionService(configuration);
    })
    .Build();

var updaterService = host.Services.GetRequiredService<INotionUpdaterService>();
await updaterService.Update(); 

await host.RunAsync();