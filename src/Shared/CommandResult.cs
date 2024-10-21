namespace Commandy.Internals
{
    internal class CommandResult : Abstractions.CommandResult
    {
        internal CommandResult(int exitCode, string output, string error) : base(exitCode, output, error)
        {
        }
    }
}
