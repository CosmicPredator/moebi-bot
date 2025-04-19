namespace Moebi.Bot.Anilist.Models.Media;

public class Media
{
    public int id { get; set; }
    public Title title { get; set; }
    public string? format { get; set; }
    public string? status { get; set; }
    public string? season { get; set; }
    public int? seasonYear { get; set; }
    public string? description { get; set; }
    public CoverImage coverImage { get; set; }
}

public class Title
{
    public string romaji { get; set; }
    public string english { get; set; }
    public string native { get; set; }
}

public class CoverImage
{
    public string? color { get; set; }
}