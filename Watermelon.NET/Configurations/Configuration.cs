using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Watermelon.NET.Configurations
{
    public class Configuration
    {
        public string Prefix { get; private set; }
        public string Token { get; private set; }
        
        public DatabaseConfiguration? DatabaseConfiguration { get; private set; }
        
        private readonly string _configurationPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");

        public Configuration()
            => LoadConfiguration();

        private void LoadConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(_configurationPath)
                .Build();

            Prefix = config.GetValue<string>(nameof(Prefix));
            Token =  config.GetValue<string>(nameof(Token));

            // TODO: Implement database features
            // var databaseConfiguration = config.GetSection(nameof(DatabaseConfiguration));
            // DatabaseConfiguration = !databaseConfiguration.Exists() ? null : new DatabaseConfiguration
            // {
            //     Host = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Host)),
            //     Port = databaseConfiguration.GetValue<ushort>(nameof(DatabaseConfiguration.Port)),
            //     Database = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Database)),
            //     Username = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Username)),
            //     Password = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Password)),
            // };
        }
    }
}