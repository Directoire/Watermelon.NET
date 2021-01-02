using DSharpPlus.Entities;

namespace Watermelon.NET.Commons.Embeds
{
    public class WatermelonEmbedBuilder
    {
        private string Title { get; set; }
        private string Description { get; set; }
        private string Footer { get; set; }
        private WatermelonTheme Theme { get; set; }

        public WatermelonEmbedBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public WatermelonEmbedBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public WatermelonEmbedBuilder WithFooter(string footer)
        {
            Footer = footer;
            return this;
        }

        public WatermelonEmbedBuilder WithTheme(WatermelonTheme theme)
        {
            Theme = theme;
            return this;
        }

        public DiscordEmbedBuilder ToEmbedBuilder()
        {
            return new DiscordEmbedBuilder()
                .WithAuthor(Title, null, Theme.Icon)
                .WithDescription(Description)
                .WithColor(Theme.Color)
                .WithFooter(Footer);
        }

        public DiscordEmbed Build()
        {
            return new DiscordEmbedBuilder()
                .WithAuthor(Title, null, Theme.Icon)
                .WithDescription(Description)
                .WithColor(Theme.Color)
                .WithFooter(Footer)
                .Build();
        }
    }
}