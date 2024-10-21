using System;

namespace Commandy.Abstractions
{
    /// <summary>
    /// Defines a standard interface for creating commands within an application.
    /// </summary>
    public interface ICommandProvider
    {
        /// <summary>
        /// Creates a command based on the specified command string.
        /// </summary>
        /// <param name="command">The command string used to create the command.</param>
        /// <returns>An instance of <see cref="ICommand"/> representing the created command.</returns>
        ICommand CreateCommand(string command);

        /// <summary>
        /// Creates a command based on the specified command string and applies options using the provided options builder.
        /// </summary>
        /// <param name="command">The command string used to create the command.</param>
        /// <param name="options">A function that configures the options for the command.</param>
        /// <returns>An instance of <see cref="ICommand"/> representing the created command with specified options.</returns>
        ICommand CreateCommand(string command, Func<ICommandOptionsBuilder, ICommandOptionsBuilder> options);
    }
}
