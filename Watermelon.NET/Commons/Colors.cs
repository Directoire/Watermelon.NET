using DSharpPlus.Entities;

namespace Watermelon.NET.Commons
{
    public static class Colors
    {
        public static readonly DiscordColor Watermelon = new(238, 61, 74);

        public static readonly DiscordColor Success = new(95, 218, 153);
        public static readonly DiscordColor Cancel = new(239, 63, 76);
        public static readonly DiscordColor Pushing = new(43, 180, 235);
        public static readonly DiscordColor Blocked = new(235, 130, 1);
        public static readonly DiscordColor Failed = new(232, 43, 58);

        public static readonly DiscordColor LowLatency = new(15, 137, 227);
        public static readonly DiscordColor NormalLatency = new(15, 137, 227);
        public static readonly DiscordColor HighLatency = new(15, 137, 227);

        public static readonly DiscordColor LowRisk = new(242, 158, 0);
        public static readonly DiscordColor MediumRisk = new(254, 195, 13);
        public static readonly DiscordColor HighRisk = new(254, 195, 13);

        public static readonly DiscordColor ToggleOff = new(233, 45, 59);
        public static readonly DiscordColor ToggleOn = new(0, 120, 212);

        public static readonly DiscordColor Settings = new(40, 162, 243);
        public static readonly DiscordColor Guilds = new(66, 164, 238);
    }
}