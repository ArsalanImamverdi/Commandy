using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Commandy.Abstractions;
using Commandy.Internals.ShellHelper;

namespace Commandy.Internals.Command
{
    internal interface ICommandProcess
    {
        Process Process { get; }
        event Abstractions.DataReceivedEventHandler DataReceived;
        event Abstractions.DataReceivedEventHandler ErrorReceived;
        int ExitCode { get; }

        Task Start(CancellationToken cancellationToken, string input = null);
        StringBuilder OutputLog { get; }
        StringBuilder ErrorLog { get; }
    }

    internal class CommandProcess : ICommandProcess
    {
        public Process Process { get; }
        private readonly TaskCompletionSource<bool> _outputReady = new TaskCompletionSource<bool>();
        private readonly TaskCompletionSource<bool> _errorOutputReady = new TaskCompletionSource<bool>();
        private readonly StringBuilder _outputLog;
        private readonly StringBuilder _errorLog;
        public event Abstractions.DataReceivedEventHandler DataReceived;
        public event Abstractions.DataReceivedEventHandler ErrorReceived;
        public CommandProcess(ICommand command, IShellHelper shellHelper)
        {
            _outputLog = new StringBuilder();
            _errorLog = new StringBuilder();
            var startInfo = new ProcessStartInfo()
            {
                FileName = shellHelper.GetExecutable(command),
                Arguments = shellHelper.GetArguments(command),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            SetEnvironmentVariables(command, startInfo);
            SetWorkingDirectory(command, startInfo);
            Process = new Process()
            {
                StartInfo = startInfo
            };
        }

        public int ExitCode => Process.ExitCode;

        public StreamReader StandardOutput => Process.StandardOutput;

        public StreamWriter StandardInput => Process.StandardInput;

        public StringBuilder OutputLog => _outputLog;

        public StringBuilder ErrorLog => _errorLog;

        public async Task Start(CancellationToken cancellationToken, string input = null)
        {
            cancellationToken.Register(() =>
            {
                if (!Process.HasExited)
                    Process.Kill();
            });
            if (!string.IsNullOrEmpty(input))
                Process.StartInfo.RedirectStandardInput = true;

            Process.Start();
            if (!string.IsNullOrEmpty(input))
            {

                await Process.StandardInput.WriteAsync(input);
                Process.StandardInput.Close();
            }
            _ = BeginReadOutput();
            _ = BeginReadError();

            Process.WaitForExit();

            await _outputReady.Task;
            await _errorOutputReady.Task;

        }

        private Task BeginReadOutput()
        {
            return BeginReadStream(Process.StandardOutput, _outputLog, DataReceived, _outputReady);
        }
        private Task BeginReadError()
        {
            return BeginReadStream(Process.StandardError, _errorLog, ErrorReceived, _errorOutputReady);
        }
        private async Task BeginReadStream(StreamReader streamReader, StringBuilder appendTo, Abstractions.DataReceivedEventHandler onDataReceived, TaskCompletionSource<bool> taskCompletionSource)
        {

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                appendTo.AppendLine(line);
                onDataReceived?.Invoke(this, new Abstractions.DataReceivedEventArgs(line));
            }
            taskCompletionSource.SetResult(true);
        }
        private static void SetEnvironmentVariables(ICommand command, ProcessStartInfo startInfo)
        {
            if (command.Options.EnvironmentVariables != null && command.Options.EnvironmentVariables.Count > 0)
            {
                startInfo.EnvironmentVariables.Clear();
                foreach (KeyValuePair<string, string> variable in command.Options.EnvironmentVariables)
                {
                    startInfo.Environment[variable.Key] = variable.Value;
                }
            }
        }
        private static void SetWorkingDirectory(ICommand command, ProcessStartInfo startInfo)
        {
            if (!string.IsNullOrEmpty(command.Options.WorkingDirectory))
                startInfo.WorkingDirectory = command.Options.WorkingDirectory;
            else
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        }


    }
}