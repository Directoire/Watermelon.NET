using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Serilog;
using Watermelon.NET.Configurations;
using Watermelon.NET.Implementation;

namespace Watermelon.NET.Modules
{
    public abstract class WatermelonModule : ModuleBase<WatermelonCommandContext>, IAsyncDisposable
    {
        protected readonly Watermelon Watermelon;
        public readonly Configuration Configuration;

        private readonly IServiceScope _scope;

        protected WatermelonModule(IServiceProvider serviceProvider)
        {
            Watermelon = serviceProvider.GetRequiredService<Watermelon>();
            Configuration = serviceProvider.GetRequiredService<Configuration>();
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_scope is IAsyncDisposable asyncDisposableScope)
                await asyncDisposableScope.DisposeAsync();
            else
                _scope.Dispose();
            
            Log.Debug($"Module: {Context.Command.Module.Name}, Command: {Context.Command.Name}, scope disposed");
        }

        /// <summary>
        /// Sends a message containing text to the Context.Channel
        /// </summary>
        /// <param name="text">The text that should be sent</param>
        /// <returns>DiscordMessage</returns>
        protected async Task<DiscordMessage> ReplyAsync(string text)
            => await Context.Channel.SendMessageAsync(text);

        /// <summary>
        /// Sends an embed to the Context.Channel
        /// </summary>
        /// <param name="embed">The embed that should be sent</param>
        /// <returns>DiscordMessage</returns>
        protected async Task<DiscordMessage> ReplyAsync(DiscordEmbed embed)
            => await Context.Channel.SendMessageAsync(embed: embed);

        /// <summary>
        /// Sends both a message and an embed to the Context.Channel
        /// </summary>
        /// <param name="text">The text that should be sent</param>
        /// <param name="embed">The embed that should be sent</param>
        /// <returns>DiscordMessage</returns>
        protected async Task<DiscordMessage> ReplyAsync(string text, DiscordEmbed embed)
            => await Context.Channel.SendMessageAsync(text, false, embed);
    }
}