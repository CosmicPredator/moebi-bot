namespace Moebi.Bot.Anilist.Models.Media;

public class MediaDetailModel : Model
{
    public Data data { get; set; }
}

public class Data
{
    public Media Media { get; set; }
}