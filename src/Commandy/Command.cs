using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commandy
{
    public class Command
    {
        public delegate void ErrorReceivedHandler(object sender, ErrorCommandEventArgs e);
        public delegate void DataReceivedHandler(object sender, DataCommandEventArgs e);
        public event DataReceivedHandler OnDataReceived;
        public event ErrorReceivedHandler OnErrorReceived;
        internal string command { get; set; }
        public string GetCommand()
        {
            return command;
        }
        public CommandOptions Options { get; }
        public static Command CreateCommand(string command, Func<CommandOptionsBuilder, CommandOptionsBuilder> commandOptionsBuilder)
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

        public CommandResult Execute(CancellationToken cancellationToken)
        {
            var executor = ExecutorFactory.GetCommandExecutor();
            return executor.Execute(this, cancellationToken);
        }
        public CommandResult Execute()
        {
            return Execute(CancellationToken.None);
        }
        public Task<CommandResult> ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }
        public Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            var executor = ExecutorFactory.GetCommandExecutor();
            return executor.ExecuteAsync(this, cancellationToken);
        }
        internal void ReceiveData(string data)
        {
            OnDataReceived?.Invoke(this, new DataCommandEventArgs() { Data = data });
        }
        internal void ReceiveError(string error)
        {
            OnErrorReceived?.Invoke(this, new ErrorCommandEventArgs() { Error = error });
        }
    }
}
