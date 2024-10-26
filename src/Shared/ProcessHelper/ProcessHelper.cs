using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Commandy.Abstractions;
using Commandy.Internals.ShellHelper;

namespace Commandy.Internals.ProcessHelper
{
    internal class ProcessHelper : IProcessHelper
    {
        private readonly IShellHelper _shellHelper;

        public ProcessHelper(IShellHelper shellHelper)
        {
            _shellHelper = shellHelper;
        }
        public Process Create(ICommand command, DataReceivedEventHandler onDataReceived, DataReceivedEventHandler onErrorReceived, CancellationToken cancellationToken, StreamReader streamReader)
        {
            var executable = _shellHelper.GetExecutable(command);
            var arguments = _shellHelper.GetArguments(command);
            var startInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (streamReader != null)
                startInfo.RedirectStandardInput = true;


            if (command.Options.EnvironmentVariables != null && command.Options.EnvironmentVariables.Count > 0)
            {
                foreach (KeyValuePair<string, string> variable in command.Options.EnvironmentVariables)
                {
                    startInfo.EnvironmentVariables.Add(variable.Key, variable.Value);
                    startInfo.Environment.Add(variable.Key, variable.Value);
                }
            }
            if (!string.IsNullOrEmpty(command.Options.WorkingDirectory))
                startInfo.WorkingDirectory = command.Options.WorkingDirectory;
            else
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            var process = new Process() { StartInfo = startInfo };

            process.Start();
            if (streamReader != null)
            {
                string input;
                while ((input = streamReader.ReadLine()) != null)
                {
                    if (!process.HasExited) // Check if the process is still running
                    {
                        process.StandardInput.WriteLine(input); // Write each line to StandardInput
                    }
                }
                process.StandardInput.Close(); // Close the input stream when done
            }

            cancellationToken.Register(() =>
            {
                process.Kill();
            });

            if (command.Options.PipeTo != null)
            {
                var pipeProcess = Create(command.Options.PipeTo, onDataReceived, onErrorReceived, cancellationToken, process.StandardOutput);

                pipeProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                        return;
                    onDataReceived(sender, e);
                };

                pipeProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                        return;
                    onErrorReceived(sender, e);
                };
                pipeProcess.WaitForExit();
            }
            else
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                        return;
                    onDataReceived(sender, e);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                        return;
                    onErrorReceived(sender, e);
                };
            }
            process.WaitForExit();
            return process;
        }
    }
}
