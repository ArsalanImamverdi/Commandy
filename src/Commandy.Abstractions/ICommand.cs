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
        /// <returns>A <see cref="ICommandResult"/> indicating the outcome of the execution.</returns>
        ICommandResult Execute();

        /// <summary>
        /// Executes the command synchronously with cancellation support.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ICommandResult"/> indicating the outcome of the execution, or indicates cancellation.</returns>
        ICommandResult Execute(CancellationToken cancellationToken);

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task{ICommandResult}"/> for non-blocking execution.</returns>
        Task<ICommandResult> ExecuteAsync();

        /// <summary>
        /// Executes the command asynchronously with cancellation support.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{ICommandResult}"/> that can be canceled.</returns>
        Task<ICommandResult> ExecuteAsync(CancellationToken cancellationToken);
    }
}
