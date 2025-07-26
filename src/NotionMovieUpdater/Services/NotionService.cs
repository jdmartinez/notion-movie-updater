using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notion.Client;
using NotionMovieUpdater.Entities;
using NotionMovieUpdater.Extensions.Notion;

namespace NotionMovieUpdater.Services;

public class NotionService : INotionService
{
    private readonly INotionClient _notionClient;
    private readonly NotionServiceSettings _settings;
    private readonly ILogger<NotionService> _logger;

    public NotionService(INotionClient notionClient, NotionServiceSettings settings, ILogger<NotionService> logger)
    {
        _notionClient = notionClient;
        _settings = settings;
        _logger = logger;
    }
    
    public async IAsyncEnumerable<Page> GetPending([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var hasMore = true;
        var nextCursor = default(string);
        var filter = new CheckboxFilter("Poster", false);
        
        while (hasMore)
        {
            var request = nextCursor is null 
                ? new DatabasesQueryParameters { Filter = filter } 
                : new DatabasesQueryParameters
                    {
                        StartCursor = nextCursor,
                        Filter = filter
                    }; 

            var queryResponse = await _notionClient.Databases.QueryAsync(_settings.DatabaseId, request, cancellationToken);
            
            if (queryResponse?.Results is not { Count: > 0 }) yield break;
            
            foreach (var page in queryResponse.Results)
            {
                yield return page as Page;
            }
            
            hasMore = queryResponse.HasMore;
            nextCursor = queryResponse.NextCursor;
        }
    }

    public async Task UpdatePage(Page page, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating cover for page {Page} with {Url}", page.ToNotionMovie().CleanTitle, page.CoverUrl());
        
        var arguments = new PagesUpdateParameters { Cover = page.Cover };
        
        await _notionClient.Pages.UpdateAsync(page.Id, arguments, cancellationToken);
    }

    public async Task UpdateProperties(Page page, IDictionary<string, PropertyValue> properties, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating page {PageId} with {Properties}", page.Id, properties);
        
        await _notionClient.Pages.UpdatePropertiesAsync(page.Id, properties, cancellationToken);
    }
}