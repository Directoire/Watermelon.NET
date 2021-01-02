using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;
using Serilog;
using Watermelon.NET.Attributes;
using Watermelon.NET.Configurations;
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
            Log.Information($"Initializing Watermelon version {Version}");
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

        private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("Pong!");
        }

        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(Watermelon) || serviceType == GetType())
                return this;

            return _provider.GetService(serviceType);
        }
    }
}