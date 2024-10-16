using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Commandy.Internals
{
    public class CommandArguments
    {
        public string Flag { get; set; }
        public string Value { get; set; }

        internal string GetArgument()
        {
            var argument = Flag;
            if (!string.IsNullOrEmpty(Value))
                argument = $"{Flag} {Value}";
            return argument;
        }
    }
    public class CommandOptions
    {
        public bool UseShell { get; set; }
        public string Command { get; set; }
        public List<CommandArguments> Arguments { get; set; } = new List<CommandArguments>();

        internal string GetArguments()
        {
            return Arguments.Select(arg => $"{arg.GetArgument()}").DefaultIfEmpty("").Aggregate((f, s) => $"{f} {s}");
        }
    }
    public class CommandOptionsBuilder
    {
        private CommandOptions options;
        public CommandOptionsBuilder()
        {
            options = new CommandOptions();
        }
        public CommandOptionsBuilder UseShell(bool useShell = true)
        {
            options.UseShell = useShell;
            return this;
        }
        public CommandOptionsBuilder AddArgument(string argument, string value)
        {
            options.Arguments.Add(new CommandArguments() { Flag = argument, Value = value });
            return this;
        }
        public CommandOptions Build()
        {
            return options;
        }
    }
    public class CommandEventArgs : EventArgs
    {

    }
    public class Command
    {
        public delegate void DataReceivedHandler(object sender, CommandEventArgs e);
        public event EventHandler OnDataReceived;
        public Command CreateCommand(string command, Func<CommandOptionsBuilder, CommandOptionsBuilder> commandOptionsBuilder)
        {
            var options = commandOptionsBuilder(new CommandOptionsBuilder()).Build();
            options.Command = command;
            return new Command(options);
        }
        public static Command CreateCommand(string command)
        {

            return new Command(new CommandOptions() { Command = command });
        }
        internal Command(CommandOptions commandOptions)
        {
            Options = commandOptions;
        }
        public CommandOptions Options { get; }
        //public virtual void OnDataReceived(string data) { }
        //public virtual void OnErrorReceived(string error) { }

        public CommandResult Execute()
        {
            var executor = ExecutorFactory.GetCommandExecutor();
            return executor.Execute(this);
        }

    }
    public class CommandResult
    {
        public int ExitCode { get; set; }
    }
    internal class ExecutorFactory
    {
        public static CommandExecutor GetCommandExecutor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsCommandExecutor();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new LinuxCommandExecutor();
            throw new NotImplementedException();
        }
    }

    internal abstract class CommandExecutor
    {
        public abstract string GetShell();
        public abstract string GetShellArgument();
        public CommandResult Execute(Command command)
        {
            string executable = GetShell();
            Process process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = $"{GetShellArgument()} {command.Options.Command} {command.Options.GetArguments()}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    //command.OnDataReceived(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    //command.OnErrorReceived(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            return new CommandResult()
            {
                ExitCode = process.ExitCode
            };
        }
    }
    internal class LinuxCommandExecutor : CommandExecutor
    {
        public override string GetShell()
        {
            throw new System.NotImplementedException();
        }

        public override string GetShellArgument()
        {
            throw new System.NotImplementedException();
        }
    }
    internal class WindowsCommandExecutor : CommandExecutor
    {
        public override string GetShell()
        {
            return "cmd.exe";
        }

        public override string GetShellArgument()
        {
            return "/C";
        }
    }
}
