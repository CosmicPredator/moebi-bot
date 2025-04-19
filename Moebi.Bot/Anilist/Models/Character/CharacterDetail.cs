namespace Moebi.Bot.Anilist.Models.Character;

public class CharacterDetailModel : Model
{
    public CharacterData data { get; set; }
}

public class CharacterData
{
    public Character Character { get; set; }
}