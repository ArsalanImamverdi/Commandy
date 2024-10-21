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
                                ICommand pipeTo,
                                TimeSpan timeout,
                                Dictionary<string, string> environmentVariables)
        {
            UseShell = useShell;
            WorkingDirectory = workingDirectory;
            PipeTo = pipeTo;
            Timeout = timeout;
            EnvironmentVariables = environmentVariables;
            Arguments = new ReadOnlyCollection<ICommandArgument>(commandArguments);
        }
        public bool UseShell { get; }

        public string WorkingDirectory { get; }

        public IReadOnlyCollection<ICommandArgument> Arguments { get; }

        public ICommand PipeTo { get; }

        public TimeSpan Timeout { get; }

        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
    }
}