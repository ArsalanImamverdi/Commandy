using Commandy.Abstractions;

namespace Commandy.Internals.Command
{
    internal class CommandArgument : ICommandArgument
    {
        public CommandArgument(string argument)
        {
            Argument = argument;
        }
        public CommandArgument(string argument, string value) : this(argument)
        {
            Value = value;
        }

        public string Argument { get; }

        public string Value { get; }
    }
}
