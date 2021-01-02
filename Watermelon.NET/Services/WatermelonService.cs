using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Watermelon.NET.Configurations;

namespace Watermelon.NET.Services
{
    public class WatermelonService
    {
        public readonly Watermelon Watermelon;
        public readonly Configuration Configuration;

        public WatermelonService(IServiceProvider serviceProvider)
        {
            Watermelon = serviceProvider.GetRequiredService<Watermelon>();
            Configuration = serviceProvider.GetRequiredService<Configuration>();
        }
        
        public Task RunTaskAsync(Task task)
            => RunTaskAsync(() => task);
        
        public Task RunTaskAsync(Func<Task> func)
        {
            Task.Run(async () =>
            {
                try
                {
                    await func.Invoke();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            });

            return Task.CompletedTask;
        }
    }
}