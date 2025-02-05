namespace NotionMovieUpdater.Entities;

public record MovieSearchResponse
{
    public IReadOnlyList<Movie> Results { get; init; } = [];
}