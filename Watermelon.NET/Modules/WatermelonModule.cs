using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Serilog;
using Watermelon.NET.Configurations;
using Watermelon.NET.Implementation;

namespace Watermelon.NET.Modules
{
    public abstract class WatermelonModule : ModuleBase<WatermelonCommandContext>, IAsyncDisposable
    {
        public readonly Watermelon Watermelon;
        public readonly Configuration Configuration;

        private readonly IServiceScope _scope;

        public WatermelonModule(IServiceProvider serviceProvider)
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
    }
}