using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Commandy.Tests.Attributes
{
    internal enum OSPlatforms : byte
    {
        Linux = 0,
        Windows = 1
    }
    internal class PlatformAttributeBase : FactAttribute
    {
        protected OSPlatform? GetOSPlatform(OSPlatforms oSPlatforms)
        {
            var prop = typeof(OSPlatform).GetProperty(oSPlatforms.ToString(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop is null)
                return null;
            var osPlatformValue = prop.GetValue(null, null);
            return osPlatformValue is OSPlatform oSPlatform ? oSPlatform : null;
        }
    }
    internal class RunOnPlatformFactAttribute : PlatformAttributeBase
    {
        public RunOnPlatformFactAttribute(OSPlatforms oSPlatforms)
        {
            if (GetOSPlatform(oSPlatforms) is OSPlatform osPlatform)
                if (!RuntimeInformation.IsOSPlatform(osPlatform))
                    Skip = $"Skipped since Not {oSPlatforms} platform";
        }
    }

    internal class SkipRunOnPlatformFactAttribute : PlatformAttributeBase
    {
        public SkipRunOnPlatformFactAttribute(params OSPlatforms[] oSPlatforms)
        {
            foreach (var oSPlatform in oSPlatforms)
            {
                if (GetOSPlatform(oSPlatform) is OSPlatform osp && RuntimeInformation.IsOSPlatform(osp))
                {
                    Skip = $"Skipped due to {oSPlatform} platform";
                    break;
                }
            }
        }
    }
}
