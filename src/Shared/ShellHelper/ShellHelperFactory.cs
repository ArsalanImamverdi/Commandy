using System;
using System.Runtime.InteropServices;

namespace Commandy.Internals.ShellHelper
{
    internal class ShellHelperFactory
    {
        public static IShellHelper GetShellHelper()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new LinuxShellHelper();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new MacOSShellHelper();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsShellHelper();
            throw new InvalidOperationException("Can not find OSPlatform");
        }
    }
}
