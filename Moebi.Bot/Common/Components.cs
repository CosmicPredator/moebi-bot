using Discord;
using Moebi.Bot.Anilist.Models.Character;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Common;

public static class Components
{
    public static MessageComponent MediaSearchSelectMenu(ref MediaSearchModel searchModel)
    {
        var selectMenu = new SelectMenuBuilder()
            .WithCustomId("media_search_select")
            .WithPlaceholder("Choose a media from list to see details")
            .WithMinValues(0);

        foreach (var media in searchModel.data.Page.media)
        {
            selectMenu.AddOption(media.title.romaji, media.id.ToString());
        }
        var component = new ComponentBuilder()
            .WithSelectMenu(selectMenu)
            .Build();
        return component;
    }
    
    public static MessageComponent CharacterSearchSelectMenu(ref CharacterSearchModel searchModel)
    {
        var selectMenu = new SelectMenuBuilder()
            .WithCustomId("character_search_select")
            .WithPlaceholder("Choose a character from list to see details")
            .WithMinValues(0);

        foreach (var character in searchModel.data.Page.characters)
        {
            selectMenu.AddOption(character.name.full, character.id.ToString());
        }
        var component = new ComponentBuilder()
            .WithSelectMenu(selectMenu)
            .Build();
        return component;
    }
}