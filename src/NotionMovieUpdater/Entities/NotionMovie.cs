namespace NotionMovieUpdater.Entities;

public record NotionMovie
{
    public string CleanTitle { get; init; }

    public MovieType Type { get; init; } = MovieType.Movie;
}