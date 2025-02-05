using Functional;
using NotionMovieUpdater.Entities;

namespace NotionMovieUpdater.Services;

public interface IMovieService
{
    Task<Option<Uri>> GetPoster(string title, MovieType movieType, CancellationToken cancellationToken = default);
}