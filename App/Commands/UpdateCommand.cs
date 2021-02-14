using System.IO;
using System.Reflection;
using App.Helpers;
using App.Models;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace App.Commands
{
    [Command(Name = "UpdateGlobalTools", FullName = "UpdateGlobalTools CLI", Description = "Update net global tools.")]
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    [SuppressDefaultHelpOption]
    public class UpdateCommand
    {
        private readonly IGlobalToolUpdater _updater;
        private readonly IConsoleRender _render;
        private readonly ILogger _logger;

        public UpdateCommand(IGlobalToolUpdater updater, IConsoleRender render, ILogger logger)
        {
            _updater = updater;
            _render = render;
            _logger = logger;
        }

        [Option("-h|--help", "Show help information.", CommandOptionType.NoValue)]
        public bool ShowHelp { get; set; }

        [Option("-f|--file", "Nuget configuration file to use.", CommandOptionType.SingleValue)]
        public string NugetConfigFile { get; set; }

        public void OnExecute(CommandLineApplication app)
        {
            if (ShowHelp)
            {
                _render.RenderTitle(Settings.GlobalToolName);
                app.ShowHelp();
                return;
            }

            if (IsNugetConfigFileEnabled() && !IsNugetConfigFileValid())
            {
                _logger.LogWarning("Nuget config file [{file}] does not exist and will be ignored", NugetConfigFile);
            }

            _updater.UpdateGlobalTools(NugetConfigFile);
        }

        private bool IsNugetConfigFileValid() => File.Exists(NugetConfigFile);

        private bool IsNugetConfigFileEnabled() => !string.IsNullOrWhiteSpace(NugetConfigFile);

        private static string GetVersion()
        {
            return typeof(UpdateCommand)
                .Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;
        }
    }
}
