using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Watermelon.NET.Commons.Weather
{
    public class WeatherResponse
    {
        public Coord Coord { get; set; }
        public Weather Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public long Visibility { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public long Dt { get; set; }
        public Sys Sys { get; set; }
        public long Timezone { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }

        public WeatherResponse ParseJson(string json)
        {
            var weatherResponse = JObject.Parse(json);
            if (weatherResponse == null)
                throw new ArgumentException($"Invalid JSON was provided");

            var coord = weatherResponse["coord"];
            if (coord != null) 
                Coord = new Coord
                {
                    Lat = coord.Value<double>("lat"), 
                    Lon = coord.Value<double>("lon")
                };

            var weather = weatherResponse["weather"]?[0];
            if (weather != null)
                Weather = new Weather
                {
                    Id = weather.Value<long>("id"),
                    Main = weather.Value<string>("main"),
                    Description = weather.Value<string>("description"),
                    Icon = $"https://openweathermap.org/img/wn/{weather.Value<string>("icon")}@2x.png"
                };

            Base = weatherResponse.Value<string>("base");

            var main = weatherResponse["main"];
            if (main != null)
                Main = new Main
                {
                    Temp = main.Value<double>("temp"),
                    FeelsLike = main.Value<double>("feels_like"),
                    TempMin = main.Value<double>("temp_min"),
                    TempMax = main.Value<double>("temp_max"),
                    Humidity = main.Value<long>("humidity"),
                    Pressure = main.Value<long>("pressure")
                };

            Visibility = weatherResponse.Value<long>("visibility");

            var wind = weatherResponse["wind"];
            if (wind != null)
                Wind = new Wind
                {
                    Deg = wind.Value<long>("deg"),
                    Speed = wind.Value<double>("speed")
                };

            var clouds = weatherResponse["clouds"];
            if (clouds != null)
                Clouds = new Clouds
                {
                    All = clouds.Value<long>("all")
                };

            Dt = weatherResponse.Value<long>("dt");

            var sys = weatherResponse["sys"];
            if (sys != null)
                Sys = new Sys
                {
                    Type = sys.Value<long>("type"),
                    Id = sys.Value<long>("id"),
                    Country = sys.Value<string>("country"),
                    Sunrise = sys.Value<long>("sunrise"),
                    Sunset = sys.Value<long>("sunset"),
                };

            Timezone = weatherResponse.Value<long>("timezone");
            Id = weatherResponse.Value<long>("id");
            Name = weatherResponse.Value<string>("name");

            return this;
        }
    }
}