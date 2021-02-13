using System.Collections.Generic;
using System.Linq;
using App.Models;
using Spectre.Console;

namespace App.Helpers
{
    public class ConsoleRender : IConsoleRender
    {
        public void RenderTitle(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new FigletText(text).LeftAligned());
            AnsiConsole.WriteLine();
        }

        public void RenderTable(ICollection<GlobalTool> globalToolsBefore, ICollection<GlobalTool> globalToolsAfter)
        {
            var count = globalToolsBefore.Count();

            var table = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.White)
                .Title($"[yellow]Found {count} global tool(s)[/]")
                .AddColumn(new TableColumn("[u]Id[/]").Centered())
                .AddColumn(new TableColumn("[u]PreviousVersion[/]").Centered())
                .AddColumn(new TableColumn("[u]CurrentVersion[/]").Centered())
                .AddColumn(new TableColumn("[u]CommandName[/]").Centered());

            foreach (var before in globalToolsBefore)
            {
                var currentVersion = GetCurrentVersion(globalToolsAfter, before);
                table.AddRow(before.Id, before.Version, currentVersion, before.Command);
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        private static string GetCurrentVersion(IEnumerable<GlobalTool> globalToolsAfter, GlobalTool before)
        {
            var previousVersion = before.Version;
            var after = globalToolsAfter.FirstOrDefault(x => x.Id == before.Id);
            var currentVersion = after?.Version ?? previousVersion;
            return currentVersion != previousVersion ? $"[green]{currentVersion}[/]" : currentVersion;
        }
    }
}