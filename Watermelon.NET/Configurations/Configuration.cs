using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Watermelon.NET.Configurations
{
    public class Configuration
    {
        private string _prefix;
        private string _token;
        private DatabaseConfiguration _databaseConfiguration;
            
        private readonly string _configurationPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
        
        public string Prefix
        {
            get => _prefix;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NullReferenceException($"Prefix must be (properly) defined in {_configurationPath}");

                _prefix = value;
            }
        }
        
        public string Token
        {
            get => _token;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NullReferenceException($"Token must be (properly) defined in {_configurationPath}");

                _token = value;
            }
        }

        public DatabaseConfiguration DatabaseConfiguration
        {
            get => _databaseConfiguration;
            set => _databaseConfiguration = value ?? throw new NullReferenceException($"Database must be (properly) defined in {_configurationPath}");
        }

        public Configuration()
            => LoadConfiguration();

        private void LoadConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(_configurationPath)
                .Build();

            Prefix = config.GetValue<string>(nameof(Prefix));
            Token =  config.GetValue<string>(nameof(Token));
            
            var databaseConfiguration = config.GetSection(nameof(DatabaseConfiguration));
            DatabaseConfiguration = !databaseConfiguration.Exists() ? null : new DatabaseConfiguration
            {
                Host = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Host)),
                Port = databaseConfiguration.GetValue<ushort>(nameof(DatabaseConfiguration.Port)),
                Database = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Database)),
                Username = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Username)),
                Password = databaseConfiguration.GetValue<string>(nameof(DatabaseConfiguration.Password)),
            };
        }
    }
}