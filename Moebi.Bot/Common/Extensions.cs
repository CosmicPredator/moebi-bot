using System.Text.RegularExpressions;

namespace Moebi.Bot.Common;


public static class Extensions
{
    public static string StripHtmlTags(this string text)
    {
        return Regex.Replace(text, "<.*?>", string.Empty);
    }
}