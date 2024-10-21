using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class WindowsShellHelper : IShellHelper
    {
        public string GetArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return ShellHelperBase.GetWindowsArguments(commandArguments);
        }
        public string GetShell()
        {
            return Constants.CMD;
        }

        public string GetShellArgument()
        {
            return Constants.CMD_ARG;
        }

        public string GetShellFlag()
        {
            return Constants.CMD_FLAG;
        }
    }
}
