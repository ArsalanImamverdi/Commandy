using Commandy.Internals;
using Commandy.Internals.CommandExecutor;
using System;
using System.Runtime.InteropServices;

namespace Commandy
{
    internal class ExecutorFactory
    {
        public static CommandExecutor GetCommandExecutor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsCommandExecutor();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxCommandExecutor();
            }
            throw new NotImplementedException();
        }
    }
}
