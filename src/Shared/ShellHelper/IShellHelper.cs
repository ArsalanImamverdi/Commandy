using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal interface IShellHelper
    {
        string GetArguments(IReadOnlyCollection<ICommandArgument> commandArguments);
        string GetShell();

        string GetShellArgument();

        string GetShellFlag();
    }
}
