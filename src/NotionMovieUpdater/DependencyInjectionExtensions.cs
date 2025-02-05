using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notion.Client;
using NotionMovieUpdater.Entities;
using NotionMovieUpdater.Services;

namespace NotionMovieUpdater;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMovieService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IMovieService, MovieService>((_, client) =>
        {
            var apiKey = configuration.GetValue<string>("TMDB_API_KEY");
            var token = configuration.GetValue<string>("TMDB_TOKEN");
            client.BaseAddress = new Uri($"https://api.themoviedb.org/3/search");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        });
        
        return services;
    }

    public static IServiceCollection AddNotionService(this IServiceCollection services, IConfiguration configuration)
        => services.AddNotionClient(options => options.AuthToken = configuration.GetValue<string>("NOTION_API_KEY"))
            .AddScoped<INotionService, NotionService>(sp =>
            {
                var notionClient = sp.GetRequiredService<INotionClient>();
                var logger = sp.GetRequiredService<ILogger<NotionService>>();
                var settings = new NotionServiceSettings { DatabaseId = configuration.GetValue<string>("NOTION_DATABASE_ID") };
                
                return new(notionClient, settings, logger);
            })
            .AddScoped<INotionUpdaterService, NotionUpdaterService>();
}