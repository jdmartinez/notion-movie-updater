using System.Net.Http.Json;
using Functional;
using Microsoft.Extensions.Logging;
using NotionMovieUpdater.Entities;

namespace NotionMovieUpdater.Services;

public class MovieService : IMovieService
{
    private const string DefaultLanguage = "es-ES";
    private const string ImagesBaseUrl = "https://image.tmdb.org/t/p/original";
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<MovieService> _logger;

    public MovieService(HttpClient httpClient, ILogger<MovieService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
     
    public async Task<Option<Uri>> GetPoster(string title, MovieType movieType, CancellationToken cancellationToken = default)
    {
        try
        {
            var route = $"{_httpClient.BaseAddress}/{movieType.ToString().ToLower()}?query={Uri.EscapeDataString(title)}&page=1";
        
            _logger.LogInformation("Getting poster from {Url}", route);
        
            var response = await _httpClient.GetFromJsonAsync<MovieSearchResponse>(route, cancellationToken);

            return response!.Results.Any()
                ? new Uri($"{ImagesBaseUrl}{response.Results[0].PosterPath}")
                : Option.None;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            return Option.None;
        }
    }
}