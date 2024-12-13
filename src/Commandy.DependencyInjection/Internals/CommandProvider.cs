using System;

using Commandy.Abstractions;
using Commandy.Internals.Command;
using Commandy.Internals.ShellHelper;

namespace Commandy.DependencyInjection.Internals
{
    internal class CommandProvider : ICommandProvider
    {
        private readonly IShellHelper _shellHelper;

        public CommandProvider(IShellHelper shellHelper)
        {
            _shellHelper = shellHelper;
        }
        public ICommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, opt => opt);
        }

        public ICommand CreateCommand(string commandText, Func<ICommandOptionsBuilder, ICommandOptionsBuilder> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var command = new Command(commandText, options(new CommandOptionsBuilder()).Build(), _shellHelper);
            return command;

        }
    }
}
