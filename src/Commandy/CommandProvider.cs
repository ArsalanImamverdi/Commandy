using System;

using Commandy.Abstractions;
using Commandy.Internals.Command;
using Commandy.Internals.ShellHelper;

namespace Commandy
{
    public class CommandProvider
    {
        public static ICommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, opt => opt);
        }

        public static ICommand CreateCommand(string commandText, Func<ICommandOptionsBuilder, ICommandOptionsBuilder> options)
        {
            var command = new Command(commandText, options(new CommandOptionsBuilder()).Build(), ShellHelperFactory.GetShellHelper());
            return command;
        }
    }
}
