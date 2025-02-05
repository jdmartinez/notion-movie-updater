using Notion.Client;

namespace NotionMovieUpdater.Services;

public interface INotionService
{
    IAsyncEnumerable<Page> GetAll(CancellationToken cancellationToken = default);
    
    Task UpdatePage(Page page, CancellationToken cancellationToken = default);
}