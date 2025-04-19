using System.Text.RegularExpressions;

namespace Moebi.Bot.Common;

/// <summary>
/// Provides general-purpose extension methods.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Removes all HTML tags from the given string.
    /// </summary>
    /// <param name="text">The input string that may contain HTML tags.</param>
    /// <returns>A cleaned string without any HTML tags.</returns>
    public static string StripHtmlTags(this string text)
    {
        return Regex.Replace(text, "<.*?>", string.Empty);
    }
}