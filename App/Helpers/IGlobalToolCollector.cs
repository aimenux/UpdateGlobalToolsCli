using System.Collections.Concurrent;
using System.Diagnostics;
using App.Models;

namespace App.Helpers
{
    public interface IGlobalToolCollector
    {
        void Reinitialize();
        ConcurrentQueue<GlobalTool> GlobalTools { get; }
        void OutputDataReceived(object _, DataReceivedEventArgs args);
    }
}