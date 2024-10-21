using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class MacOSShellHelper : IShellHelper
    {
        public string GetArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return ShellHelperBase.GetUnixArguments(commandArguments);
        }

        public string GetShell()
        {
            return Constants.BASH;
        }

        public string GetShellArgument()
        {
            return Constants.SHELL_ARG;
        }

        public string GetShellFlag()
        {
            return Constants.SHELL_FLAG;
        }
    }
}
