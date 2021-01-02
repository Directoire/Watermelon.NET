#nullable enable
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;
using Serilog;
using Watermelon.NET.Attributes;
using Watermelon.NET.Configurations;
using Watermelon.NET.Data.Context;
using Watermelon.NET.Services;

namespace Watermelon.NET
{
    public class Watermelon : IServiceProvider
    {
        public const string Version = "0.0.1";
        
        public DiscordClient Client { get; private set; }
        public DiscordUser? CurrentUser => Client.CurrentUser;
        public int Latency => Client.Ping;

        private readonly Configuration _configuration;
        private readonly IServiceProvider _provider;

        public Watermelon()
        {
            #if DEBUG
            Log.Information($"Initializing development Watermelon version {Version}");
            #else
            Log.Information($"Initializing production Watermelon version {Version}");
            #endif
            
            _configuration = new Configuration();
            var logFactory = new LoggerFactory().AddSerilog();
            
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = _configuration.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Debug,
                LoggerFactory = logFactory,
                Intents = DiscordIntents.All
            });
            
            var commandService = new CommandService(new CommandServiceConfiguration
            {
                DefaultRunMode = RunMode.Parallel,
                StringComparison = StringComparison.InvariantCultureIgnoreCase
            });

            var databaseConnection = GetDatabaseConnection();

            var watermelonServices = typeof(Watermelon).Assembly.GetTypes()
                .Where(x => typeof(WatermelonService).IsAssignableFrom(x)
                            && !x.GetTypeInfo().IsInterface
                            && !x.GetTypeInfo().IsAbstract);
            
            var services = new ServiceCollection();
            foreach (var serviceType in watermelonServices)
                services.AddSingleton(serviceType);
            
            _provider = services
                .AddSingleton(this)
                .AddSingleton(_configuration)
                .AddSingleton(commandService)
                .AddDbContext<WatermelonContext>(x => 
                    x.UseNpgsql(databaseConnection))
                .BuildServiceProvider();

            var autoStartServices = typeof(Watermelon).Assembly.GetTypes()
                .Where(x => typeof(WatermelonService).IsAssignableFrom(x)
                            && x.GetCustomAttribute<AutoStartAttribute>() != null
                            && !x.GetTypeInfo().IsInterface
                            && !x.GetTypeInfo().IsAbstract);

            foreach (var serviceType in autoStartServices)
                _provider.GetRequiredService(serviceType);
            
            Log.Information($"Finished initialization, added {autoStartServices.Count()} service(s) with the [AutoStart] attribute to the service collection");
        }

        public Task ConnectAsync()
            => Client.ConnectAsync();

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(Watermelon) || serviceType == GetType())
                return this;

            return _provider.GetService(serviceType);
        }
        
        private string GetDatabaseConnection()
        {
            var connectionString = new StringBuilder();
            connectionString.Append("Host=").Append(_configuration.DatabaseConfiguration.Host).Append(';');

            if (_configuration.DatabaseConfiguration.Port > 0)
                connectionString.Append("Port=").Append(_configuration.DatabaseConfiguration.Port).Append(';');

            connectionString.Append("Username=").Append(_configuration.DatabaseConfiguration.Username).Append(';')
                .Append("Password=").Append(_configuration.DatabaseConfiguration.Password).Append(';')
                .Append("Database=").Append(_configuration.DatabaseConfiguration.Database).Append(';');

            return connectionString.ToString();
        }
    }
}