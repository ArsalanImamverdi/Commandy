using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Commandy.Abstractions;
using Commandy.Internals.ProcessHelper;

namespace Commandy.Internals.Command
{
    internal class Command : ICommand
    {
        private readonly IProcessHelper _processHelper;

        private readonly StringBuilder _data;
        private readonly StringBuilder _error;
        public event DataReceivedEventHandler OnDataReceived;
        public event DataReceivedEventHandler OnErrorReceived;

        public string CommandText { get; private set; }

        public ICommandOptions Options { get; private set; }

        internal Command(IProcessHelper processHelper)
        {
            _processHelper = processHelper;
            _data = new StringBuilder();
            _error = new StringBuilder();
        }

        internal void Initialize(string commandText, ICommandOptions options)
        {
            CommandText = commandText;
            Options = options;
        }

        public ICommandResult Execute()
        {
            return Execute(CancellationToken.None);
        }

        public ICommandResult Execute(CancellationToken cancellationToken)
        {
            OnDataReceived += (sender, e) =>
            {
                _data.AppendLine(e.Data);
            };
            OnErrorReceived += (sender, e) =>
            {
                _error.AppendLine(e.Data);
            };

            var process = _processHelper.Create(this, OnDataReceived, OnErrorReceived, cancellationToken, null);

            return new CommandResult(process.ExitCode, _data.ToString(), _error.ToString());
        }

        public Task<ICommandResult> ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }

        public Task<ICommandResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => Execute(cancellationToken));
        }
    }
}
