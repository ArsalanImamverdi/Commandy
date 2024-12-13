namespace Commandy.Abstractions
{
    public enum CommandChainType : byte
    {
        And = 0,
        Or = 1,
        Pipe = 2
    }

    public interface IChainedCommand
    {
        int Priority { get; }
        ICommand Command { get; }
        CommandChainType ChainType { get; }
    }
}
