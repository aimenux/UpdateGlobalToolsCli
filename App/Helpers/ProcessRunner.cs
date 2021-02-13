using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using SystemProcess = System.Diagnostics.Process;

namespace App.Helpers
{
    public class ProcessRunner : IProcessRunner
    {
        private readonly ILogger _logger;

        public ProcessRunner(ILogger logger)
        {
            _logger = logger;
        }

        public void RunProcess(string name, string arguments)
        {
            void OutputDataReceived(object _, DataReceivedEventArgs args) => LogProcessError(args.Data);
            void ErrorDataReceived(object _, DataReceivedEventArgs args) => LogProcessInfo(args.Data);
            RunProcess(name, arguments, OutputDataReceived, ErrorDataReceived);
        }

        public void RunProcess(string name, string arguments, DataReceivedEventHandler outputDataReceived)
        {
            void ErrorDataReceived(object _, DataReceivedEventArgs args) => LogProcessInfo(args.Data);
            RunProcess(name, arguments, outputDataReceived, ErrorDataReceived);
        }

        public void RunProcess(
            string name,
            string arguments,
            DataReceivedEventHandler outputDataReceived,
            DataReceivedEventHandler errorDataReceived)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = $@"{name}",
                Arguments = $@"{arguments}"
            };

            var process = new SystemProcess
            {
                StartInfo = startInfo
            };

            process.ErrorDataReceived += errorDataReceived;
            process.OutputDataReceived += outputDataReceived;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();
            process.Close();
        }

        private void LogProcessInfo(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            var keywords = new[]
            {
                "fail",
                "échec",
                "impossible"
            };

            if (keywords.Any(x => message.Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning(message);
                return;
            }

            _logger.LogInformation(message);
        }

        private void LogProcessError(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            if (message.Contains("error", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError(message);
                return;
            }

            if (message.Contains("warning", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(message);
                return;
            }

            if (message.Contains("version '", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(message);
                return;
            }

            _logger.LogTrace(message);
        }
    }
}