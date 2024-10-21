using System;

using Commandy.Abstractions;
using Commandy.Internals.Command;
using Commandy.Internals.ProcessHelper;

namespace Commandy.DependencyInjection.Internals
{
    internal class CommandProvider : ICommandProvider
    {
        private readonly IProcessHelper _processHelper;

        public CommandProvider(IProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }
        public ICommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, opt => opt);
        }

        public ICommand CreateCommand(string commandText, Func<ICommandOptionsBuilder, ICommandOptionsBuilder> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var command = new Command(_processHelper);
            command.Initialize(commandText, options(new CommandOptionsBuilder()).Build());
            return command;
        }
    }
}
