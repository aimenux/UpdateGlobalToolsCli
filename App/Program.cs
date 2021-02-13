using System.IO;
using System.Threading.Tasks;
using App.Commands;
using App.Helpers;
using App.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace App
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).RunCommandLineApplicationAsync<UpdateCommand>(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConsoleLogger();
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddConfiguration(GetLoggingSection(hostingContext));
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<UpdateCommand>();
                    services.AddTransient<IProcessRunner, ProcessRunner>();
                    services.AddTransient<IConsoleRender, ConsoleRender>();
                    services.AddTransient<IGlobalToolUpdater, GlobalToolUpdater>();
                    services.AddTransient<IGlobalToolCollector, GlobalToolCollector>();
                });

        private static IConfigurationSection GetLoggingSection(HostBuilderContext hostingContext)
        {
            return hostingContext.Configuration.GetSection(Settings.LoggingSectionName);
        }

        private static void AddConsoleLogger(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.IncludeScopes = true;
                options.TimestampFormat = "[HH:mm:ss:fff] ";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
            });
        }

        private static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
        {
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger(Settings.GlobalToolName);
            });
        }
    }
}
