using System;
using System.Threading.Tasks;
using Qmmands;

namespace Watermelon.NET.Modules
{
    [Name("General")]
    public partial class General : WatermelonModule
    {
        private readonly IServiceProvider _serviceProvider;
        
        public General(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        [Command("ping")]
        public async Task PingAsync()
        {
            await Context.Channel.SendMessageAsync($"Pong! `{Watermelon.Latency} ms`");
        }
    }
}