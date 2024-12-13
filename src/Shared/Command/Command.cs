using System.Threading;
using System.Threading.Tasks;

using Commandy.Abstractions;
using Commandy.Internals.ShellHelper;

namespace Commandy.Internals.Command
{
    internal class Command : ICommand
    {
        private readonly IShellHelper _shellHelper;
        public event DataReceivedEventHandler OnDataReceived;
        public event DataReceivedEventHandler OnErrorReceived;

        public string CommandText { get; private set; }

        public ICommandOptions Options { get; private set; }

        internal Command(string commandText, ICommandOptions commandOptions, IShellHelper shellHelper)
        {
            CommandText = commandText;
            Options = commandOptions;
            _shellHelper = shellHelper;
        }

        public ICommandResult Execute()
        {
            return Execute(CancellationToken.None);
        }

        public ICommandResult Execute(CancellationToken cancellationToken)
        {
            var commandRunner = CommandRunnerFactory.GetCommandRunner(this, _shellHelper);
            try
            {
                if (OnDataReceived != null)
                    commandRunner.CommandProcess.DataReceived += (s, d) => OnDataReceived?.Invoke(s, d);
                if (OnErrorReceived != null)
                    commandRunner.CommandProcess.ErrorReceived += (s, d) => OnErrorReceived?.Invoke(s, d);
                return commandRunner.Run(cancellationToken);
            }
            finally
            {
                if (OnDataReceived != null)
                    commandRunner.CommandProcess.DataReceived -= (s, d) => OnDataReceived?.Invoke(s, d);
                if (OnErrorReceived != null)
                    commandRunner.CommandProcess.ErrorReceived -= (s, d) => OnErrorReceived?.Invoke(s, d);
            }
        }

        public Task<ICommandResult> ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }

        public Task<ICommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var commandRunner = CommandRunnerFactory.GetCommandRunner(this, _shellHelper);
            try
            {
                if (OnDataReceived != null)
                    commandRunner.CommandProcess.DataReceived += (s, d) => OnDataReceived?.Invoke(s, d);
                if (OnErrorReceived != null)
                    commandRunner.CommandProcess.ErrorReceived += (s, d) => OnErrorReceived?.Invoke(s, d);
                return commandRunner.RunAsync(cancellationToken);
            }
            finally
            {
                if (OnDataReceived != null)
                    commandRunner.CommandProcess.DataReceived -= (s, d) => OnDataReceived?.Invoke(s, d);
                if (OnErrorReceived != null)
                    commandRunner.CommandProcess.ErrorReceived -= (s, d) => OnErrorReceived?.Invoke(s, d);
            }

        }
    }
}
