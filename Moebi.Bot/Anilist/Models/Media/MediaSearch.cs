namespace Moebi.Bot.Anilist.Models.Media;

public class MediaSearchModel : Model
{
    public MediaSearchData data { get; set; }
}

public class MediaSearchData
{
    public Page Page { get; set; }
}

public class Page
{
    public PageInfo pageInfo { get; set; }
    public Media[] media { get; set; }
}

public class PageInfo
{
    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }
}