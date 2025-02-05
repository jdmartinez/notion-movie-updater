using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notion.Client;
using NotionMovieUpdater.Entities;

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
    
    public async IAsyncEnumerable<Page> GetAll([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var hasMore = true;
        var nextCursor = default(string);
        
        while (hasMore)
        {
            var request = nextCursor is null 
                ? new DatabasesQueryParameters() 
                : new DatabasesQueryParameters
                    {
                        StartCursor = nextCursor,
                    }; 

            var queryResponse = await _notionClient.Databases.QueryAsync(_settings.DatabaseId, request, cancellationToken);
            
            if (queryResponse?.Results is not { Count: > 0 }) yield break;
            
            foreach (var page in queryResponse.Results)
            {
                yield return page;
            }
            
            hasMore = queryResponse.HasMore;
            nextCursor = queryResponse.NextCursor;
        }
    }

    public async Task UpdatePage(Page page, CancellationToken cancellationToken = default)
    {
        if (page is null) return;

        var arguments = new PagesUpdateParameters
        {
            Cover = page.Cover,
        };
        
        await _notionClient.Pages.UpdateAsync(page.Id, arguments, cancellationToken);
    }
}