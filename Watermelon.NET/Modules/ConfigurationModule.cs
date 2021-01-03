using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using Qmmands;
using Watermelon.NET.Commons;
using Watermelon.NET.Commons.Embeds;
using Watermelon.NET.Data.Models;

namespace Watermelon.NET.Modules
{
    [Name("Configuration")]
    public class ConfigurationModule : WatermelonModule
    {
        public ConfigurationModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        [Qmmands.Command("prefix")]
        public async Task PrefixAsync(string prefix = null)
        {
            var guild = await DbContext.Guilds
                .FindAsync(Context.Guild.Id);
            
            if (prefix == null)
            {
                await Context.Channel.TriggerTypingAsync();
                var embed = new WatermelonEmbedBuilder()
                    .WithTitle("Prefix")
                    .WithDescription(
                        $"The prefix of Watermelon is currently set to the `{(guild == null ? Configuration.Prefix : guild.Prefix)}` prefix.")
                    .WithTheme(Themes.Settings)
                    .Build();

                await ReplyAsync(embed: embed);
                return;
            }

            var updating = new WatermelonEmbedBuilder()
                .WithTitle("Updating the prefix...")
                .WithTheme(Themes.Pushing)
                .Build();

            var message = await ReplyAsync(embed: updating);

            if (prefix.Length > 8)
            {
                var error = new WatermelonEmbedBuilder()
                    .WithTitle("Length too long")
                    .WithDescription("The length of the new prefix must be 8 or less characters.")
                    .WithTheme(Themes.Cancel)
                    .Build();

                await message.ModifyAsync(embed: error);
                return;
            }
            
            if (guild != null)
                guild.Prefix = prefix;
            else
                DbContext.Add(new Guild {Id = Context.Guild.Id, Prefix = prefix});

            bool changed = await DbContext.SaveChangesAsync() > 0;
            if (!changed)
            {
                var error = new WatermelonEmbedBuilder()
                    .WithTitle("Failed to update")
                    .WithDescription("An error occurred while trying to update the prefix.")
                    .WithTheme(Themes.Failed)
                    .Build();

                await message.ModifyAsync(embed: error);
                return;
            }

            var success = new WatermelonEmbedBuilder()
                .WithTitle("Successfully updated prefix")
                .WithDescription($"The prefix has been updated to the `{prefix}` prefix.")
                .WithTheme(Themes.Success)
                .Build();

            await message.ModifyAsync(embed: success);
        }
    }
}