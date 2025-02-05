namespace NotionMovieUpdater.Entities;

public record NotionServiceSettings
{
    public required string DatabaseId { get; init; }
};