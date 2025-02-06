using Notion.Client;
using NotionMovieUpdater.Entities;

namespace NotionMovieUpdater.Extensions.Notion;

public static class PageExtensions
{
    public static bool HasCoverImage(this Page page)
        => page?.Cover is not null;
    
    public static Uri CoverUrl(this Page page)
        => new(((ExternalFile)page.Cover)?.External.Url);

    public static NotionMovie ToNotionMovie(this Page page)
        => new NotionMovie
        {
            CleanTitle = page.GetPropertyValueOrEmpty("Título limpio"),
            Type = page.GetPropertyValueOrEmpty("Tipo") switch
            {
                "Serie" => MovieType.TV,
                _ => MovieType.Movie
            },
        };
    
    public static Page AddCover(this Page page, Uri uri)
    {
        page.Cover = new ExternalFile
        {
            External = new ExternalFile.Info { Url = uri.ToString() }
        };

        return page;
    }
    
    private static string GetPropertyValueOrEmpty(this Page page, string propertyName)
        => page.Properties.TryGetValue(propertyName, out var value)
            ? value switch
                {
                    FormulaPropertyValue formula => formula.Formula.String,
                    SelectPropertyValue select => select.Select.Name,
                    _ => string.Empty,
                }
            : string.Empty;
}