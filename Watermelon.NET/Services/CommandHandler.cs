using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Serilog;
using Watermelon.NET.Attributes;
using Watermelon.NET.Data.Context;
using Watermelon.NET.Implementation;

namespace Watermelon.NET.Services
{
    [AutoStart]
    public class CommandHandler : WatermelonService
    {
        private readonly CommandService _commandService;

        public CommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _commandService = serviceProvider.GetRequiredService<CommandService>();

            Watermelon.Client.MessageCreated += OnMessageCreated;
            
            var assembly = Assembly.GetAssembly(typeof(Watermelon));
            _commandService.AddModules(assembly);
        }

        private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Message.MessageType != MessageType.Default) return;
            if (e.Message.Author.IsBot) return;
            
            await RunTaskAsync(ExecuteAsync(sender, e));
        }

        private async Task ExecuteAsync(DiscordClient client, MessageCreateEventArgs args)
        {
            if (args.Guild != null)
            {
                var channelPermissions = args.Guild.CurrentMember.PermissionsIn(args.Channel);
                if (!channelPermissions.HasPermission(Permissions.SendMessages))
                    return;
            }

            var prefix = await GetGuildPrefixAsync(args.Guild);
            if (!CommandUtilities.HasPrefix(args.Message.Content, prefix, StringComparison.InvariantCultureIgnoreCase, out var output))
            {
                if (client.CurrentUser == null)
                    return;

                if (!CommandUtilities.HasPrefix(args.Message.Content, client.CurrentUser.Username, StringComparison.InvariantCultureIgnoreCase, out output)
                    && !args.Message.HasMentionPrefix(client.CurrentUser, out output))
                    return;
            }

            var context = new WatermelonCommandContext(Watermelon, args.Message, prefix);
            var result = await _commandService.ExecuteAsync(output, context);

            if (result.IsSuccessful)
                return;

            Log.Error(result.ToString());
            
            // TODO: Implement error handling
            await context.Channel.SendMessageAsync(result.ToString());
        }
        
        private async Task<string> GetGuildPrefixAsync(DiscordGuild? guild)
        {
            if (guild is null)
                return Configuration.Prefix;

            using var scope = Watermelon.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WatermelonContext>();
            var prefix = (await db.Guilds.FindAsync(guild.Id))?.Prefix;
            
            return !string.IsNullOrEmpty(prefix) ? prefix : Configuration.Prefix;
        }
    }
}