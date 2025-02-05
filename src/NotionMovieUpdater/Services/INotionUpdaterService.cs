namespace NotionMovieUpdater.Services;

public interface INotionUpdaterService
{
    Task Update(CancellationToken cancellationToken = default);
}