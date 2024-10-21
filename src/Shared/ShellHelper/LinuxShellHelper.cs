using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class LinuxShellHelper : IShellHelper
    {
        public string GetArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return ShellHelperBase.GetUnixArguments(commandArguments);
        }
        public string GetShell()
        {
            return System.IO.File.Exists(Constants.BASH) ? Constants.BASH : Constants.SH;
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
