using System;
using System.Threading.Tasks;
using Qmmands;
using Watermelon.NET.Commons;
using Watermelon.NET.Commons.Embeds;

namespace Watermelon.NET.Modules
{
    [Name("General")]
    public class GeneralModule : WatermelonModule
    {
        public GeneralModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        [Command("ping", "latency")]
        public async Task PingAsync()
        {
            var latency = Watermelon.Latency;

            var theme = latency switch
            {
                < 200 => Themes.LowLatency,
                > 1000 => Themes.HighLatency,
                _ => Themes.NormalLatency
            };

            var embed = new WatermelonEmbedBuilder()
                .WithTitle("Latency")
                .WithDescription($"Watermelon currently has a latency of `{latency} ms`.")
                .WithTheme(theme)
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}