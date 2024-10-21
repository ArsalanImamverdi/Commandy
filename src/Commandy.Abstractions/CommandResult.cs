namespace Commandy.Abstractions
{
    /// <summary>
    /// Represents the result of executing a command, encapsulating the exit code, output, and error information.
    /// </summary>
    public partial class CommandResult
    {
        protected internal CommandResult(int exitCode, string output, string error)
        {
            ExitCode = exitCode;
            Output = output;
            Error = error;
        }
        /// <summary>
        /// Gets the exit code returned by the command execution.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the standard output generated during command execution.
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// Gets the error output generated during command execution, if any.
        /// </summary>
        public string Error { get; }
    }
}
