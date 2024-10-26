using System.Collections.Generic;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal interface IShellHelper
    {
        string GetArguments(ICommand command);

        string GetExecutable(ICommand command);
        string GetShell();

        string GetShellArgument();

        string GetShellFlag();
    }
}
