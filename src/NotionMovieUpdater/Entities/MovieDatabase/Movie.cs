using System.Text.Json.Serialization;

namespace NotionMovieUpdater.Entities;

public record Movie
{
    public int Id { get; init; }
    
    public string Title { get; init; } = default!;
    
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; init; }
}