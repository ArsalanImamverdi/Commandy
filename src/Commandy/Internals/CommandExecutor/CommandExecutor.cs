using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Commandy.Internals.CommandExecutor
{
    internal abstract class CommandExecutor
    {
        private readonly char[] EnvironmentNewLine = Environment.NewLine.ToCharArray();
        private readonly StringBuilder _data = new StringBuilder();
        private readonly StringBuilder _error = new StringBuilder();
        public abstract string GetShell();
        public abstract string GetShellArgument();
        public CommandResult Execute(Command command, CancellationToken cancellationToken)
        {
            var exitCode = GetProcess(command, cancellationToken);
            return new CommandResult()
            {
                ExitCode = exitCode,
                Output = _data.ToString().TrimEnd(EnvironmentNewLine),
                Error = _error.ToString().TrimEnd(EnvironmentNewLine)
            };
        }
        public Task<CommandResult> ExecuteAsync(Command command, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var exitCode = GetProcess(command, cancellationToken);

                return new CommandResult()
                {
                    ExitCode = exitCode,
                    Output = _data.ToString().TrimEnd(EnvironmentNewLine),
                    Error = _error.ToString().TrimEnd(EnvironmentNewLine)
                };
            });
        }
        private int GetProcess(Command command, CancellationToken cancellationToken)
        {
            string executable = command.Options.UseShell ? $"{GetShell()}" : command.Options.Command;
            string arguments = command.Options.UseShell ? $"{GetShellArgument()} {command.Options.Command} {command.Options.GetArguments()}" : command.Options.GetArguments();
            command.command = $"{executable} {arguments}";
            var startInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (command.Options.EnvironmentVariables != null && command.Options.EnvironmentVariables.Count > 0)
            {
                foreach (KeyValuePair<string, string> variable in command.Options.EnvironmentVariables)
                    startInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
            }
            if (!string.IsNullOrEmpty(command.Options.WorkingDirectory))
                startInfo.WorkingDirectory = command.Options.WorkingDirectory;

            Process process = new Process()
            {
                StartInfo = startInfo,
            };

            process.OutputDataReceived += (sender, args) =>
            {
                AppendData(args.Data, command, DataReceivedType.Data);
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                AppendData(args.Data, command, DataReceivedType.Error);
            };
            cancellationToken.Register(() =>
            {
                if (process.HasExited) return;
                process.Kill();
            });
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return process.ExitCode;
        }
        private void AppendData(string data, Command command, DataReceivedType dataReceivedType)
        {
            if (!string.IsNullOrEmpty(data))
                switch (dataReceivedType)
                {
                    case DataReceivedType.Error:
                        _error.AppendLine(data);
                        command.ReceiveError(data);
                        break;
                    case DataReceivedType.Data:
                        _data.AppendLine(data);
                        command.ReceiveData(data);
                        break;
                }
        }
        enum DataReceivedType : byte
        {
            Data = 0,
            Error = 1
        }
    }
}
