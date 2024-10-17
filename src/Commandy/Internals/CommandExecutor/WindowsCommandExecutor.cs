namespace Commandy.Internals.CommandExecutor
{
    internal class WindowsCommandExecutor : CommandExecutor
    {
        public override string GetShell()
        {
            return "cmd.exe";
        }

        public override string GetShellArgument()
        {
            return "/C";
        }
    }
}
