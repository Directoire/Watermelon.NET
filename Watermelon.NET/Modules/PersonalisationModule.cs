using System;
using System.Threading.Tasks;
using Qmmands;
using Watermelon.NET.Commons;
using Watermelon.NET.Commons.Embeds;
using Watermelon.NET.Data.Models;
using UnitsType = Watermelon.NET.Commons.Units.UnitsType;

namespace Watermelon.NET.Modules
{
    [Name("personalisation")]
    public class PersonalisationModule : WatermelonModule
    {
        public PersonalisationModule(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Command("units")]
        public async Task UnitsAsync(string type = null)
        {
            var user = await DbContext.Users
                .FindAsync(Context.User.Id);

            if (type == null)
            {
                await Context.Channel.TriggerTypingAsync();
                var current = new WatermelonEmbedBuilder()
                    .WithTitle("Units")
                    .WithDescription(user == null ? 
                        "You currently don't have a preference set for units." :
                        $"You've set your preferred units to `{user.Units.ToString()}`.")
                    .WithTheme(Themes.Settings)
                    .Build();

                await ReplyAsync(embed: current);
                return;
            }

            var updating = new WatermelonEmbedBuilder()
                .WithTitle("Updating units preference...")
                .WithTheme(Themes.Pushing)
                .Build();

            var message = await ReplyAsync(embed: updating);

            if (!Units.TryParse(type, out UnitsType result))
            {
                var error = new WatermelonEmbedBuilder()
                    .WithTitle("Invalid units type")
                    .WithDescription("Please pick between `metric`, `imperial` or `kelvin`.")
                    .WithTheme(Themes.Cancel)
                    .Build();

                await message.ModifyAsync(embed: error);
                return;
            }

            if (user != null)
                user.Units = result;
            else
                DbContext.Add(new User {Id = Context.User.Id, Units = result});

            var changed = await DbContext.SaveChangesAsync() > 0;
            if (!changed)
            {
                var failed = new WatermelonEmbedBuilder()
                    .WithTitle("Failed to update")
                    .WithDescription("An error occurred while trying to update the units preference.")
                    .WithTheme(Themes.Failed)
                    .Build();

                await message.ModifyAsync(embed: failed);
                return;
            }

            var success = new WatermelonEmbedBuilder()
                .WithTitle("Successfully updated units preference")
                .WithDescription($"Watermelon will now use `{result.ToString()}` units when you use a command.")
                .WithTheme(Themes.Success)
                .Build();

            await message.ModifyAsync(embed: success);
        }
    }
}