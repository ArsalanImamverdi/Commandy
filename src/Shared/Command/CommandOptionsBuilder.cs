using System;
using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.Command
{
    internal class CommandOptionsBuilder : ICommandOptionsBuilder
    {
        private readonly List<ICommandArgument> _commandArguments = new List<ICommandArgument>();
        private readonly List<IChainedCommand> _chainedCommands = new List<IChainedCommand>();
        private readonly Dictionary<string, string> _environmentVariables = new Dictionary<string, string>();
        private string _workingDirectory = null;
        private TimeSpan _timeout = TimeSpan.MaxValue;
        private bool _useShell = true;
        private int _priority = 0;
        public ICommandOptionsBuilder AddArgument(string argument)
        {
            _commandArguments.Add(new CommandArgument(argument));
            return this;
        }

        public ICommandOptionsBuilder AddArgument(string argument, string value)
        {
            _commandArguments.Add(new CommandArgument(argument, value));
            return this;
        }

        public ICommandOptionsBuilder AddEnvironmentVariable(string key, string value)
        {
            if (_environmentVariables.ContainsKey(key))
                _environmentVariables.Remove(key);
            _environmentVariables.Add(key, value);
            return this;
        }

        public ICommandOptionsBuilder ChainTo(ICommand command, CommandChainType commandChainType)
        {
            _chainedCommands.Add(new ChainedCommand(command, commandChainType, _priority++));
            return this;
        }

        public ICommandOptionsBuilder CloneFrom(ICommand command)
        {
            throw new NotImplementedException();
        }

        public ICommandOptionsBuilder CloneFrom(ICommandOptions commandOptions)
        {
            throw new NotImplementedException();
        }

        public ICommandOptionsBuilder PipeTo(ICommand command)
        {
            _chainedCommands.Add(new ChainedCommand(command, CommandChainType.Pipe, _priority++));
            return this;
        }

        public ICommandOptionsBuilder Timeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public ICommandOptionsBuilder UseShell(bool useShell = true)
        {
            _useShell = useShell;
            return this;
        }

        public ICommandOptionsBuilder WorkingDirectory(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
            return this;
        }
        public ICommandOptions Build()
        {
            return new CommandOptions(_useShell, _workingDirectory, _commandArguments, _timeout, _environmentVariables, _chainedCommands);
        }
    }
}