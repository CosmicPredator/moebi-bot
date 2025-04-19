namespace Moebi.Bot.Anilist.Models.Character;

public class CharacterSearchModel : Model
{
    public Data data { get; set; }
}

public class Data
{
    public Page Page { get; set; }
}

public class Page
{
    public PageInfo pageInfo { get; set; }
    public Character[] characters { get; set; }
}

public class PageInfo
{
    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }
}
