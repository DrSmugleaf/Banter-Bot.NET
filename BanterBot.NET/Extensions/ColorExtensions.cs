using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace BanterBot.NET.Extensions
{
    public static class ColorExtensions
    {
        public static bool TryParseColor(string colorString, [NotNullWhen(true)] out Color color)
        {
            try
            {
                color = ColorTranslator.FromHtml(colorString);
                return true;
            }
            catch
            {
                color = default;
                return false;
            }
        }

        public static bool TryParseDiscordColor(string colorString, [NotNullWhen(true)] out Discord.Color color)
        {
            try
            {
                color = (Discord.Color) ColorTranslator.FromHtml(colorString);
                return true;
            }
            catch
            {
                color = default;
                return false;
            }
        }
    }
}
