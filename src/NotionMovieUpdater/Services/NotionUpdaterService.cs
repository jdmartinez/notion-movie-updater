using Functional;
using Microsoft.Extensions.Logging;
using Notion.Client;
using NotionMovieUpdater.Extensions.Notion;

namespace NotionMovieUpdater.Services;

public class NotionUpdaterService : INotionUpdaterService
{
    private readonly IMovieService _movieService;
    private readonly INotionService _notionService;
    private readonly ILogger<NotionUpdaterService> _logger;

    public NotionUpdaterService(IMovieService movieService, INotionService notionService,  ILogger<NotionUpdaterService> logger)
    {
        _movieService = movieService;
        _notionService = notionService;
        _logger = logger;
    }


    public async Task Update(CancellationToken cancellationToken = default)
    {
        var pages = _notionService
            .GetAll(cancellationToken)
            .Where(page => !page.HasCoverImage());
        
        await foreach (var page in pages)
        {
            var posterUrl = await GetPosterUrl(page, cancellationToken);

            if (!posterUrl.IsSome) continue;

            page.AddCover(posterUrl.Value);
            await _notionService.UpdatePage(page, cancellationToken);
        }
    }

    private async Task<Option<Uri>> GetPosterUrl(Page page, CancellationToken cancellationToken = default)
    {
        var movie = page.ToNotionMovie();
        
        _logger.LogInformation("Getting poster url for {Title}", movie.CleanTitle);
        
        var posterUri = await _movieService.GetPoster(movie.CleanTitle, movie.Type, cancellationToken);

        return posterUri;
    }
}