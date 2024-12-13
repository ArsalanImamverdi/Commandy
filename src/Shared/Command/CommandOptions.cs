using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Commandy.Abstractions;

namespace Commandy.Internals.Command
{
    internal class CommandOptions : ICommandOptions
    {
        internal CommandOptions(bool useShell,
                                string workingDirectory,
                                List<ICommandArgument> commandArguments,
                                TimeSpan timeout,
                                Dictionary<string, string> environmentVariables,
                                List<IChainedCommand> chainedCommands)
        {
            UseShell = useShell;
            WorkingDirectory = workingDirectory;
            Timeout = timeout;
            EnvironmentVariables = environmentVariables;
            Arguments = new ReadOnlyCollection<ICommandArgument>(commandArguments);
            ChainedCommands = chainedCommands;
        }
        public bool UseShell { get; }

        public string WorkingDirectory { get; }

        public IReadOnlyCollection<ICommandArgument> Arguments { get; }

        public IChainedCommand PipeTo { get; }

        public TimeSpan Timeout { get; }

        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }

        public IReadOnlyList<IChainedCommand> ChainedCommands { get; }

        public bool PreserveLog { get; }
    }
}