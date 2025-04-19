using System.Globalization;
using System.Text.RegularExpressions;

namespace Moebi.Bot.Common;

/// <summary>
/// Provides general-purpose extension methods.
/// </summary>
public static class Extensions
{
    private static Random random = new Random();
    
    /// <summary>
    /// Removes all HTML tags from the given string.
    /// </summary>
    /// <param name="text">The input string that may contain HTML tags.</param>
    /// <returns>A cleaned string without any HTML tags.</returns>
    public static string StripHtmlTags(this string text)
    {
        return Regex.Replace(text, "<.*?>", string.Empty);
    }

    public static string GetRandomHexColor()
    {
        int red = random.Next(0, 256);   // 0 to 255
        int green = random.Next(0, 256);
        int blue = random.Next(0, 256);

        return $"#{red:X2}{green:X2}{blue:X2}";
    }

    public static string? ToTitleCase(this string? text)
    {
        if (text is null) return null;
        text = text.Replace("_", " ");
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text.ToLower());
    }
}