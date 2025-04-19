using Discord;
using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Models.Character;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Common;

public static class Embeds
{
    private const string AnilistLogoUrl =
        "https://upload.wikimedia.org/wikipedia/commons/thumb/6/61/AniList_logo.svg/2048px-AniList_logo.svg.png";
    public static Embed PingEmbed(ref int latency)
    {
        return new EmbedBuilder()
            .WithTitle("Moebi Bot")
            .WithDescription("Ping successful!")
            .WithColor(Color.Green)
            .WithFields(
                new EmbedFieldBuilder()
                    .WithName("Latency")
                    .WithValue($"{latency}ms"))
            .Build();
    }

    public static Embed MediaDetailEmbed(ref MediaDetailModel mediaDetail)
    {
        var fields = new List<EmbedFieldBuilder>()
        {
            new EmbedFieldBuilder()
                .WithName("Format")
                .WithValue(mediaDetail.data.Media.format!.Replace("_", " "))
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Status")
                .WithValue(mediaDetail.data.Media.status)
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Season")
                .WithValue($"{mediaDetail.data.Media.season} {mediaDetail.data.Media.seasonYear}")
                .WithIsInline(true),
        };

        var embedBuilder = new EmbedBuilder()
            .WithTitle(mediaDetail.data.Media.title.romaji)
            .WithFields(fields)
            .WithDescription(mediaDetail.data.Media.description!.StripHtmlTags())
            .WithThumbnailUrl(AnilistLogoUrl)
            .WithImageUrl($"https://img.anili.st/Media/{mediaDetail.data.Media.id}");

        if (string.IsNullOrEmpty(mediaDetail.data.Media.coverImage.color))
            embedBuilder.WithColor(Color.Parse(mediaDetail.data.Media.coverImage.color));
        
        return embedBuilder.Build();
    }

    public static Embed MediaSearchEmbed(ref MediaSearchModel mediaSearch, string searchQuery)
    {
        var description = "";
        foreach (var media in mediaSearch.data.Page.media)
        {
            description += $"**{media.id}.** {media.title.romaji} _({media.format})_\n";
        }
        return new EmbedBuilder()
            .WithTitle($"Results: {searchQuery}")
            .WithColor(Color.Green)
            .WithDescription(description)
            .WithThumbnailUrl(AnilistLogoUrl)
            .Build();
    }

    public static Embed CharacterSearchEmbed(ref CharacterSearchModel characterDetail, string searchQuery)
    {
        var description = "";
        foreach (var character in characterDetail.data.Page.characters)
        {
            var alternativeName = string.Empty;
            if (character.name.alternative.Length != 0) alternativeName = $"_({character.name.alternative[0]})_"; 
            description += $"**{character.id}.** {character.name.full} {alternativeName}\n";
        }
        return new EmbedBuilder()
            .WithTitle($"Results: {searchQuery}")
            .WithColor(Color.Green)
            .WithDescription(description)
            .WithThumbnailUrl(AnilistLogoUrl)
            .Build();
    }
}

