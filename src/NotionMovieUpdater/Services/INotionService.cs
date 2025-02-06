using Notion.Client;

namespace NotionMovieUpdater.Services;

public interface INotionService
{
    IAsyncEnumerable<Page> GetPending(CancellationToken cancellationToken = default);
    
    Task UpdatePage(Page page, CancellationToken cancellationToken = default);
    
    Task UpdateProperties(Page page, IDictionary<string, PropertyValue> properties, CancellationToken cancellationToken = default);
}