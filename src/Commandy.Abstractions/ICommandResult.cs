namespace Commandy.Abstractions
{
    /// <summary>
    /// Represents the result of executing a command, encapsulating the exit code, output, and error information.
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// Gets the exit code returned by the command execution.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// Gets the standard output generated during command execution.
        /// </summary>
        string Output { get; }

        /// <summary>
        /// Gets the error output generated during command execution, if any.
        /// </summary>
        string Error { get; }
    }
}
