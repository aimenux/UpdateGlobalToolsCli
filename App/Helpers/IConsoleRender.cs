using System.Collections.Generic;
using App.Models;

namespace App.Helpers
{
    public interface IConsoleRender
    {
        void RenderTitle(string text);
        void RenderTable(ICollection<GlobalTool> globalToolsBefore, ICollection<GlobalTool> globalToolsAfter);
    }
}
