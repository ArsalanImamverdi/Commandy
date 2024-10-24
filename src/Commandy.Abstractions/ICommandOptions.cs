using System;
using System.Collections.Generic;

namespace Commandy.Abstractions
{
    /// <summary>
    /// Holding options to run a command
    /// </summary>
    public interface ICommandOptions
    {
        /// <summary>
        /// Gets a value indicating whether to start a new shell and run the command within.
        /// When <c>true</c>, the command is executed in a new shell process.
        /// </summary>
        bool UseShell { get; }

        /// <summary>
        /// Gets the working directory to run the command in.
        /// If not set, the current working directory is used.
        /// </summary>
        string WorkingDirectory { get; }

        /// <summary>
        /// Gets the arguments to run the command with.
        /// </summary>
        IReadOnlyCollection<ICommandArgument> Arguments { get; }

        /// <summary>
        /// Gets the command to pipe the output to, if any.
        /// When set, the output of the current command is passed to the specified command.
        /// </summary>
        ICommand PipeTo { get; }

        /// <summary>
        /// Gets the timeout for the command.
        /// If not set, the command runs indefinitely.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Gets the environment variables to set for the command.
        /// These variables are only valid for the duration of the command and do not affect the parent process.
        /// </summary>
        IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

    }

    /// <summary>
    /// A builder interface for creating <see cref="ICommandOptions"/> instances.
    /// </summary>
    public interface ICommandOptionsBuilder
    {
        /// <summary>
        /// Specifies whether to start a new shell and run the command within.
        /// </summary>
        /// <param name="useShell">A value indicating whether to use a shell. Defaults to <c>true</c>.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder UseShell(bool useShell = true);

        /// <summary>
        /// Sets the working directory to run the command in.
        /// </summary>
        /// <param name="workingDirectory">The path to the working directory.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder WorkingDirectory(string workingDirectory);

        /// <summary>
        /// Adds a command-line argument without a value.
        /// </summary>
        /// <param name="argument">The argument to add.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder AddArgument(string argument);

        /// <summary>
        /// Adds a command-line argument with a value.
        /// </summary>
        /// <param name="argument">The argument to add.</param>
        /// <param name="value">The value of the argument.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder AddArgument(string argument, string value);

        /// <summary>
        /// Pipes the output of the command to another command.
        /// </summary>
        /// <param name="command">The command to pipe the output to.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder PipeTo(ICommand command);

        /// <summary>
        /// Sets the timeout for the command.
        /// </summary>
        /// <param name="timeout">The timeout value.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder Timeout(TimeSpan timeout);

        /// <summary>
        /// Adds an environment variable to set for the command.
        /// </summary>
        /// <param name="environmentVariable">The name of the environment variable.</param>
        /// <param name="value">The value of the environment variable.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder AddEnvironmentVariable(string environmentVariable, string value);

        /// <summary>
        /// Clones the options from another command.
        /// </summary>
        /// <param name="command">The command to clone options from.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder CloneFrom(ICommand command);

        /// <summary>
        /// Clones the options from another <see cref="ICommandOptions"/> instance.
        /// </summary>
        /// <param name="commandOptions">The options to clone from.</param>
        /// <returns>The current builder instance.</returns>
        ICommandOptionsBuilder CloneFrom(ICommandOptions commandOptions);

        /// <summary>
        /// Builds the <see cref="ICommandOptions"/> instance.
        /// </summary>
        /// <returns>The built <see cref="ICommandOptions"/> instance.</returns>
        ICommandOptions Build();
    }
}