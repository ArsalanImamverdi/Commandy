using Commandy.Abstractions;

namespace Commandy.Internals.Command
{
    internal class CommandResult : ICommandResult
    {
        public CommandResult(int exitCode, string output, string error)
        {
            ExitCode = exitCode;
            Output = output;
            Error = error;
        }
        public int ExitCode { get; }

        public string Output { get; }

        public string Error { get; }
    }
}