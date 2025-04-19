namespace Moebi.Bot.Anilist.Models.Media;

public class Media
{
    public int id { get; set; }
    public int idMal { get; set; }
    public Title title { get; set; }
    public string? format { get; set; }
    public string description { get; set; }
    public string? status { get; set; }
    public string? season { get; set; }
    public int? seasonYear { get; set; }
    public int? favourites { get; set; }
    public int? popularity { get; set; }
    public int? episodes { get; set; }
    public int? chapters { get; set; }
    
    public int? duration { get; set; }
    public int? volumes { get; set; }
    public int? averageScore { get; set; }
    public string siteUrl { get; set; }
    public string type { get; set; }
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
    public string color { get; set; }
}