using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Watermelon.NET
{
    class Program
    {
        private static readonly string LogPath = Path.Combine(Environment.CurrentDirectory, "logs/watermelon-.log");
        
        static async Task Main(string[] args)
        { 
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Log.Logger = new LoggerConfiguration()
            #if DEBUG
                .MinimumLevel.Debug()
            #else
                .MinimumLevel.Information()
            #endif
                .WriteTo.Console(theme: SystemConsoleTheme.Literate)
                .WriteTo.Async(x => x.File(LogPath,  shared: true, rollingInterval: RollingInterval.Hour))
                .CreateLogger();

            await new Watermelon().ConnectAsync();
            await Task.Delay(-1);
        }
        
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.ExceptionObject.ToString());
            Log.CloseAndFlush();
        }
    }
}