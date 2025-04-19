namespace Moebi.Bot.Anilist.Models.Character;

public class Characters
{
    public string age { get; set; }
    public Name name { get; set; }
    public string bloodType { get; set; }
    public DateOfBirth dateOfBirth { get; set; }
    public string description { get; set; }
    public int favourites { get; set; }
    public string gender { get; set; }
    public int id { get; set; }
    public Image image { get; set; }
}

public class Name
{
    public string userPreferred { get; set; }
    public string[] alternative { get; set; }
    public object[] alternativeSpoiler { get; set; }
    public string first { get; set; }
    public string last { get; set; }
    public string full { get; set; }
}

public class DateOfBirth
{
    public int? day { get; set; }
    public int? month { get; set; }
    public object year { get; set; }
}

public class Image
{
    public string large { get; set; }
}