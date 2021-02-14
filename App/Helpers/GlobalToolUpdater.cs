using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Models;

namespace App.Helpers
{
    public class GlobalToolUpdater : IGlobalToolUpdater
    {
        private readonly IProcessRunner _runner;
        private readonly IConsoleRender _render;
        private readonly IGlobalToolCollector _collector;

        public GlobalToolUpdater(IProcessRunner runner, IConsoleRender render, IGlobalToolCollector collector)
        {
            _runner = runner;
            _render = render;
            _collector = collector;
        }

        public void UpdateGlobalTools(string nugetConfigFile)
        {
            var globalToolsBefore = GetGlobalTools();
            UpdateGlobalTools(globalToolsBefore, nugetConfigFile);
            var globalToolsAfter = GetGlobalTools();
            _render.RenderTable(globalToolsBefore, globalToolsAfter);
        }

        private void UpdateGlobalTools(IEnumerable<GlobalTool> globalTools, string nugetConfigFile)
        {
            const string name = @"dotnet";
            var arguments = File.Exists(nugetConfigFile)
                ? $"tool update -g {{0}} --configfile {nugetConfigFile}"
                : @"tool update -g {0} --ignore-failed-sources";

            foreach (var globalTool in globalTools.Where(x => !x.IsCurrentTool))
            {
                _runner.RunProcess(name, string.Format(arguments, globalTool.Id));
            }
        }

        private ICollection<GlobalTool> GetGlobalTools(bool reinitialize = true)
        {
            if (reinitialize)
            {
                _collector.Reinitialize();
            }

            const string name = @"dotnet";
            const string arguments = @"tool list -g";
            _runner.RunProcess(name, arguments, _collector.OutputDataReceived);
            return _collector
                .GlobalTools
                .ToList();
        }
    }
}