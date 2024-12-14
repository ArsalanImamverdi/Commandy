using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commandy.Abstractions;
using Commandy.Internals.ShellHelper;

namespace Commandy.Internals.Command
{
    internal interface IChainedCommandRunner : ICommandRunner
    {
        ICommandRunner ChainedCommandRunner { get; }
    }

    internal interface ICommandRunner
    {
        ICommandProcess CommandProcess { get; }
        ICommandResult Run(CancellationToken cancellationToken, string input = null);
        Task<ICommandResult> RunAsync(CancellationToken cancellationToken, string input = null);
    }
    internal class CommandRunnerFactory
    {

        public static ICommandRunner GetCommandRunner(ICommand command, IShellHelper shellHelper)
        {
            ICommandRunner commandRunner = new CommandRunner(command, shellHelper);
            if (command.Options?.ChainedCommands != null && command.Options.ChainedCommands.Count > 0)
            {
                foreach (IChainedCommand chainedCommand in command.Options.ChainedCommands.OrderBy(cmd => cmd.Priority))
                {
                    ICommand currentCommand = command;

                    if (chainedCommand.ChainType == CommandChainType.Pipe)
                    {
                        commandRunner = new PipedCommandRunner(commandRunner, GetCommandRunner(chainedCommand.Command, shellHelper));
                    }
                    else
                    {
                        commandRunner = new ChainedCommandRunnerClass(commandRunner, GetCommandRunner(chainedCommand.Command, shellHelper), chainedCommand.ChainType);
                    }
                }

            }
            return commandRunner;
        }
    }
    internal class CommandRunner : ICommandRunner
    {
        public ICommandProcess CommandProcess { get; }
        public ICommand Command { get; }
        public CommandRunner(ICommand command, IShellHelper shellHelper)
        {
            Command = command;
            CommandProcess = new CommandProcess(command, shellHelper);
        }
        public virtual ICommandResult Run(CancellationToken cancellationToken, string input = null)
        {
            return RunAsync(cancellationToken, input).GetAwaiter().GetResult();
        }

        public virtual async Task<ICommandResult> RunAsync(CancellationToken cancellationToken, string input = null)
        {
            await CommandProcess.StartAsync(cancellationToken, input);
            return new CommandResult(CommandProcess.ExitCode, CommandProcess.OutputLog.ToString(), CommandProcess.ErrorLog.ToString());
        }
    }
    internal class ChainedCommandRunnerClass : IChainedCommandRunner
    {
        private readonly CommandChainType _commandChainType;
        public ICommandProcess CommandProcess { get; }
        public ICommandRunner CommandRunner { get; }

        public ICommandRunner ChainedCommandRunner { get; }

        public ChainedCommandRunnerClass(ICommandRunner commandRunner, ICommandRunner chainedCommandRunner, CommandChainType commandChainType)
        {
            _commandChainType = commandChainType;
            CommandRunner = commandRunner;
            ChainedCommandRunner = chainedCommandRunner;
            CommandProcess = commandRunner.CommandProcess;
        }
        public virtual async Task<ICommandResult> RunAsync(CancellationToken cancellationToken, string input = null)
        {
            var result = await CommandRunner.RunAsync(cancellationToken, input);
            if (_commandChainType == CommandChainType.And && result.ExitCode == 0)
            {
                var chainResult = await ChainedCommandRunner.RunAsync(cancellationToken, input);
                return new CommandResult(chainResult.ExitCode, string.Join(Environment.NewLine, result.Output, chainResult.Output), string.Join(Environment.NewLine, result.Error, chainResult.Error));
            }
            if (_commandChainType == CommandChainType.Or && result.ExitCode != 0)
            {
                var chainResult = await ChainedCommandRunner.RunAsync(cancellationToken, input);
                return new CommandResult(chainResult.ExitCode, string.Join(Environment.NewLine, result.Output, chainResult.Output), string.Join(Environment.NewLine, result.Error, chainResult.Error));
            }
            return new CommandResult(result.ExitCode, result.Output.ToString(), result.Error.ToString());
        }
        public virtual ICommandResult Run(CancellationToken cancellationToken, string input = null)
        {
            return RunAsync(cancellationToken, input).GetAwaiter().GetResult();
        }
    }
    internal class PipedCommandRunner : ChainedCommandRunnerClass
    {
        public PipedCommandRunner(ICommandRunner commandRunner, ICommandRunner chainedCommandRunner) : base(commandRunner, chainedCommandRunner, CommandChainType.Pipe)
        {
        }

        public override async Task<ICommandResult> RunAsync(CancellationToken cancellationToken, string input = null)
        {
            var commandRunnerResult = await CommandRunner.RunAsync(cancellationToken);
            if (commandRunnerResult.ExitCode != 0)
            {
                return commandRunnerResult;
            }
            var result = await ChainedCommandRunner.RunAsync(cancellationToken, commandRunnerResult.Output);
            return new CommandResult(result.ExitCode, result.Output.ToString(), result.Error.ToString());

        }

    }

}
