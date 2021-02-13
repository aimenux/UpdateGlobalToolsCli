using System;

namespace App.Models
{
    public class GlobalTool
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public string Command { get; set; }

        public bool IsCurrentTool => string.Equals(Id, Settings.GlobalToolName, StringComparison.OrdinalIgnoreCase);
    }
}
