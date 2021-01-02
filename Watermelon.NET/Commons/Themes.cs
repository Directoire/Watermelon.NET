using Watermelon.NET.Commons.Embeds;

namespace Watermelon.NET.Commons
{
    public class Themes
    {
        public static readonly WatermelonTheme Watermelon = new(Icons.Watermelon, Colors.Watermelon);

        public static readonly WatermelonTheme Success = new(Icons.Success, Colors.Success);
        public static readonly WatermelonTheme Cancel = new(Icons.Cancel, Colors.Cancel);
        public static readonly WatermelonTheme Pushing = new(Icons.Pushing, Colors.Pushing);
        public static readonly WatermelonTheme Blocked = new(Icons.Blocked, Colors.Blocked);
        public static readonly WatermelonTheme Failed = new(Icons.Failed, Colors.Failed);

        public static readonly WatermelonTheme LowLatency = new(Icons.LowLatency, Colors.LowLatency);
        public static readonly WatermelonTheme NormalLatency = new(Icons.NormalLatency, Colors.NormalLatency);
        public static readonly WatermelonTheme HighLatency = new(Icons.HighLatency, Colors.HighLatency);

        public static readonly WatermelonTheme LowRisk = new(Icons.LowRisk, Colors.LowRisk);
        public static readonly WatermelonTheme MediumRisk = new(Icons.MediumRisk, Colors.MediumRisk);
        public static readonly WatermelonTheme HighRisk = new(Icons.HighRisk, Colors.HighRisk);

        public static readonly WatermelonTheme ToggleOff = new(Icons.ToggleOff, Colors.ToggleOff);
        public static readonly WatermelonTheme ToggleOn = new(Icons.ToggleOn, Colors.ToggleOn);

        public static readonly WatermelonTheme Settings = new(Icons.Settings, Colors.Settings);
        public static readonly WatermelonTheme Guilds = new(Icons.Guilds, Colors.Guilds);
    }
}