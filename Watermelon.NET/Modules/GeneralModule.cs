using System;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Qmmands;
using Serilog;
using Watermelon.NET.Commons;
using Watermelon.NET.Commons.Embeds;
using UnitsType = Watermelon.NET.Commons.Units.UnitsType;

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

        [Command("weather")]
        public async Task WeatherAsync([Remainder] string city)
        {
            await Context.Channel.TriggerTypingAsync();

            var parts = city.ToLower().Split(" ");
            var units = Units.Parse(parts.Last());
            city = city.Replace(units.ToString().ToLower(), "");

            var client = new HttpClient();
            var cityUrlEncoded = UrlEncoder.Default.Encode(city);
            var response = await client.GetAsync(
                $"https://api.openweathermap.org/data/2.5/weather?q={cityUrlEncoded}&units={units.ToString()}&appid={Configuration.OpenWeatherKey}");

            Log.Debug(response.StatusCode.ToString());
            
            if (!response.IsSuccessStatusCode)
            {
                var error = new WatermelonEmbedBuilder()
                    .WithTitle("An error occurred")
                    .WithDescription("Watermelon was unable to fetch the weather of that city.")
                    .WithTheme(Themes.Failed)
                    .Build();

                await ReplyAsync(embed: error);
                return;
            }

            var responseString = await response.Content.ReadAsStringAsync();
            await ReplyAsync(responseString);
        }
    }
}