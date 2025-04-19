using System.Text;
using Discord;
using Moebi.Bot.Anilist.Models;
using Moebi.Bot.Anilist.Models.Character;
using Moebi.Bot.Anilist.Models.Media;

namespace Moebi.Bot.Common;

/// <summary>
/// Provides utility methods for building various Discord embed messages for the bot.
/// </summary>
public static class Embeds
{
    /// <summary>
    /// The URL to the AniList logo used as a thumbnail in embed messages.
    /// </summary>
    private const string AnilistLogoUrl =
        "https://upload.wikimedia.org/wikipedia/commons/thumb/6/61/AniList_logo.svg/2048px-AniList_logo.svg.png";
    
    /// <summary>
    /// Creates a simple "ping" embed displaying the bot's latency.
    /// </summary>
    /// <param name="latency">The measured latency in milliseconds.</param>
    /// <returns>A green-colored <see cref="Embed"/> showing the latency.</returns>
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

    /// <summary>
    /// Creates a detailed embed for a specific media (anime/manga) using AniList data.
    /// </summary>
    /// <param name="mediaDetail">The media detail model containing the media's information.</param>
    /// <returns>An <see cref="Embed"/> with media information, cover image, and description.</returns>
    public static Embed MediaDetailEmbed(ref MediaDetailModel mediaDetail)
    {
        List<EmbedFieldBuilder> embedFields = [new EmbedFieldBuilder()
                .WithName("Format")
                .WithValue(mediaDetail.data.Media.format == null ? "N/A" : mediaDetail.data.Media.format.ToTitleCase())
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Status")
                .WithValue(mediaDetail.data.Media.status.ToTitleCase() ?? "N/A")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Season")
                .WithValue($"{mediaDetail.data.Media.season.ToTitleCase() ?? "N/A"} {mediaDetail.data.Media.seasonYear.ToString() ?? "N/A"}")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Type")
                .WithValue(mediaDetail.data.Media.type.ToTitleCase())
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Favourites")
                .WithValue(mediaDetail.data.Media.favourites.ToString() ?? "N/A")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Average Score")
                .WithValue(mediaDetail.data.Media.averageScore.ToString() ?? "N/A")
                .WithIsInline(true)];

        switch (mediaDetail.data.Media.type)
        {
            case "ANIME":
                embedFields.Add(new EmbedFieldBuilder()
                    .WithName("Total Episodes")
                    .WithValue(mediaDetail.data.Media.episodes.ToString() ?? "N/A")
                    .WithIsInline(true));
                embedFields.Add(new EmbedFieldBuilder()
                    .WithName("Duration")
                    .WithValue($"{mediaDetail.data.Media.duration.ToString()}mins/episode" ?? "N/A")
                    .WithIsInline(true));
                break;
            case "MANGA":
                embedFields.Add(new EmbedFieldBuilder()
                    .WithName("Total Chapters")
                    .WithValue(mediaDetail.data.Media.chapters.ToString() ?? "N/A")
                    .WithIsInline(true));
                embedFields.Add(new EmbedFieldBuilder()
                    .WithName("Total Volumes")
                    .WithValue(mediaDetail.data.Media.volumes.ToString() ?? "N/A")
                    .WithIsInline(true));
                break;
        }

        var descriptionString = new StringBuilder();
        descriptionString.Append($"[AniList]({mediaDetail.data.Media.siteUrl}) | [MAL](https://myanimelist.net/anime/{mediaDetail.data.Media.idMal})");
        descriptionString.Append("\n\n");
        descriptionString.Append($"_{mediaDetail.data.Media.description!.StripHtmlTags()}_");

        var embedBuilder = new EmbedBuilder()
            .WithTitle(mediaDetail.data.Media.title.romaji)
            .WithFields(embedFields)
            .WithDescription(descriptionString.ToString())
            .WithColor(Color.Parse(Extensions.GetRandomHexColor()))
            .WithThumbnailUrl(AnilistLogoUrl)
            .WithImageUrl($"https://img.anili.st/Media/{mediaDetail.data.Media.id}");

        if (!string.IsNullOrEmpty(mediaDetail.data.Media.coverImage.color))
            embedBuilder.WithColor(Color.Parse(mediaDetail.data.Media.coverImage.color));
        
        return embedBuilder.Build();
    }

    /// <summary>
    /// Creates a search results embed listing media found for a given query.
    /// </summary>
    /// <param name="mediaSearch">The media search result model.</param>
    /// <param name="searchQuery">The original search query entered by the user.</param>
    /// <returns>An <see cref="Embed"/> listing media titles and formats.</returns>
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

    /// <summary>
    /// Creates a search results embed listing characters found for a given query.
    /// </summary>
    /// <param name="characterDetail">The character search result model.</param>
    /// <param name="searchQuery">The original search query entered by the user.</param>
    /// <returns>An <see cref="Embed"/> listing character names and alternative names.</returns>
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

    public static Embed CharacterDetailEmbed(ref CharacterDetailModel details)
    {
        string dob = "N/A";
        var dobData = details.data.Character.dateOfBirth;

        if (dobData.day is not null && dobData.month is not null && dobData.year is not null)
        {
            dob = $"{dobData.month}/{dobData.day}/{dobData.year}";
        }
        
        List<EmbedFieldBuilder> embedFields = [
            new EmbedFieldBuilder()
                .WithName("Age")
                .WithValue(details.data.Character.age ?? "N/A")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Blood Type")
                .WithValue(details.data.Character.bloodType ?? "N/A")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Gender")
                .WithValue(details.data.Character.gender ?? "N/A")
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("DOB")
                .WithValue(dob)
                .WithIsInline(true),
            new EmbedFieldBuilder()
                .WithName("Favourites")
                .WithValue(details.data.Character.favourites.ToString() ?? "N/A")
                .WithIsInline(true)
        ];
        
        StringBuilder descriptionBuilder = new StringBuilder();
        descriptionBuilder.Append($"_{details.data.Character.description!.StripHtmlTags()}_");
        descriptionBuilder.Append("\n\n");
        descriptionBuilder.Append("**Alternative Names**\n");
        if (details.data.Character.name.alternative.Length != 0)
            descriptionBuilder.AppendJoin("• ", details.data.Character.name.alternative);
        else
            descriptionBuilder.Append("_None_");
        descriptionBuilder.Append("\n\n");
        descriptionBuilder.Append("**Alternative Spoiler Names**\n");
        if (details.data.Character.name.alternativeSpoiler.Length != 0)
            descriptionBuilder.AppendJoin(" • ", details.data.Character.name.alternativeSpoiler);
        else
            descriptionBuilder.Append("_None_");
        descriptionBuilder.Append("\n\n");

        var embedBuilder = new EmbedBuilder()
            .WithTitle(details.data.Character.name.userPreferred)
            .WithDescription(descriptionBuilder.ToString())
            .WithColor(Color.Parse(Extensions.GetRandomHexColor()))
            .WithThumbnailUrl(AnilistLogoUrl)
            .WithFields(embedFields)
            .WithImageUrl(details.data.Character.image.large);
        return embedBuilder.Build();
    }
}

