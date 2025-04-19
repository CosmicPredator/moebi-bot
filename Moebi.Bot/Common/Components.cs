using Discord;
using Moebi.Bot.Anilist.Models.Character;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Common;

/// <summary>
/// Provides reusable Discord message components (like select menus) for media and character search results.
/// </summary>
public static class Components
{
    /// <summary>
    /// Builds a select menu component allowing the user to choose a media (anime/manga) from search results.
    /// </summary>
    /// <param name="searchModel">The media search model containing the list of media options.</param>
    /// <returns>A Discord <see cref="MessageComponent"/> representing the select menu.</returns>
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
    
    /// <summary>
    /// Builds a select menu component allowing the user to choose a character from search results.
    /// </summary>
    /// <param name="searchModel">The character search model containing the list of character options.</param>
    /// <returns>A Discord <see cref="MessageComponent"/> representing the select menu.</returns>
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