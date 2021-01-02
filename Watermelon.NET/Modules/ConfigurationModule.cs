using System;
using System.Threading.Tasks;
using Qmmands;
using Watermelon.NET.Data.Models;

namespace Watermelon.NET.Modules
{
    [Name("Configuration")]
    public class ConfigurationModule : WatermelonModule
    {
        public ConfigurationModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        [Command("prefix")]
        public async Task PrefixAsync(string prefix = null)
        {
            var guild = await DbContext.Guilds
                .FindAsync(Context.Guild.Id);
            
            if (prefix == null)
            {
                await ReplyAsync($"The current prefix is set to `{(guild == null ? Configuration.Prefix : guild.Prefix)}`.");
                return;
            }

            if (guild != null)
                guild.Prefix = prefix;
            else
                DbContext.Add(new Guild {Id = Context.Guild.Id, Prefix = prefix});

            await DbContext.SaveChangesAsync();

            await ReplyAsync($"Modified prefix to `{prefix}`.");
        }
    }
}