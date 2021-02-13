using System.Diagnostics;

namespace App.Helpers
{
    public interface IProcessRunner
    {
        void RunProcess(string name, string arguments);
        void RunProcess(string name, string arguments, DataReceivedEventHandler outputDataReceived);
        void RunProcess(string name, string arguments, DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived);
    }
}
