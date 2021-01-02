using DSharpPlus.Entities;

namespace Watermelon.NET.Commons.Embeds
{
    public class WatermelonTheme
    {
        public string Icon { get; }
        public DiscordColor Color { get; }

        public WatermelonTheme(string icon, DiscordColor color)
        {
            Icon = icon;
            Color = color;
        }
    }
}