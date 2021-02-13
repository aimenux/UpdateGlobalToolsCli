using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using App.Models;

namespace App.Helpers
{
    public class GlobalToolCollector : IGlobalToolCollector
    {
        public void Reinitialize()
        {
            GlobalTools.Clear();
        }

        public ConcurrentQueue<GlobalTool> GlobalTools { get; } = new ();

        public void OutputDataReceived(object _, DataReceivedEventArgs args)
        {
            var message = args.Data;
            var globalTool = ExtractGlobalTool(message);
            if (globalTool == null) return;
            GlobalTools.Enqueue(globalTool);
        }

        private static GlobalTool ExtractGlobalTool(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return null;

            const string separator = " ";
            var parts = message
                .Split(separator)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            if (parts.Length != 3) return null;

            return new GlobalTool
            {
                Id = parts[0],
                Version = parts[1],
                Command = parts[2]
            };
        }
    }
}