using System.IO;

namespace Commandy.Internals.CommandExecutor
{
    internal class LinuxCommandExecutor : CommandExecutor
    {
        private const string BASH = "/bin/bash";
        private const string SH = "/bin/sh";
        public override string GetShell()
        {
            return File.Exists(BASH) ? BASH : SH;
        }

        public override string GetShellArgument()
        {
            return "-c";
        }
    }
}
