using System;
using System.Threading.Tasks;
using Qmmands;

namespace Watermelon.NET.Modules
{
    public class Configuration : WatermelonModule
    {
        public Configuration(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        [Command("prefix")]
        public async Task PrefixAsync()
        {
            
        }
    }
}