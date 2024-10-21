using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Commandy.Abstractions
{
    /// <summary>
    /// Defines a standard interface for executing commands within an application.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Occurs when data is received during command execution.
        /// </summary>
        event DataReceivedEventHandler OnDataReceived;

        /// <summary>
        /// Occurs when an error is encountered during command execution.
        /// </summary>
        event DataReceivedEventHandler OnErrorReceived;

        /// <summary>
        /// Gets the textual representation of the command.
        /// </summary>
        string CommandText { get; }

        /// <summary>
        /// Gets the options associated with the command.
        /// </summary>
        ICommandOptions Options { get; }

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the execution.</returns>
        CommandResult Execute();

        /// <summary>
        /// Executes the command synchronously with cancellation support.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the execution, or indicates cancellation.</returns>
        CommandResult Execute(CancellationToken cancellationToken);

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{CommandResult}"/> for non-blocking execution.</returns>
        Task<CommandResult> ExecuteAsync();

        /// <summary>
        /// Executes the command asynchronously with cancellation support.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{CommandResult}"/> that can be canceled.</returns>
        Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
