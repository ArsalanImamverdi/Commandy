using Commandy.Abstractions;

namespace Commandy.Internals.Command
{
    internal class ChainedCommand : IChainedCommand
    {
        internal ChainedCommand(ICommand command, CommandChainType chainType, int priority)
        {
            Command = command;
            ChainType = chainType;
            Priority = priority;
        }

        public CommandChainType ChainType { get; }

        public ICommand Command { get; }

        public int Priority { get; }
    }
}