namespace Commandy.Abstractions
{
    /// <summary>
    /// Represents an argument for a command, encapsulating its name and value.
    /// </summary>
    public interface ICommandArgument
    {
        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        string Argument { get; }

        /// <summary>
        /// Gets the value associated with the argument.
        /// </summary>
        string Value { get; }
    }
}
