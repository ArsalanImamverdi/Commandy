using System.Runtime.InteropServices;

namespace Commandy.Tests.Attributes
{
    public enum OSPlatforms : byte
    {
        Linux = 0,
        Windows = 1
    }
    public class PlatformFactAttribute : FactAttribute
    {
        protected  OSPlatform? GetOSPlatform(OSPlatforms oSPlatforms)
        {
            var prop = typeof(OSPlatform).GetProperty(oSPlatforms.ToString(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (prop is null)
                return null;
            var osPlatformValue = prop.GetValue(null);
            if (osPlatformValue is OSPlatform osPlatform) return osPlatform;
            return null;
        }
    }
    public class RunOnPlatformFactAttribute : PlatformFactAttribute
    {
        public RunOnPlatformFactAttribute(OSPlatforms oSPlatform)
        {
            var oSPlatformValue = GetOSPlatform(oSPlatform);
            if (oSPlatformValue is not OSPlatform platform || !RuntimeInformation.IsOSPlatform(platform))
            {
                Skip = $"Ignored on OsPlatform incompatibility";
            }
        }
    }
    public class IgnoreOnPlatformFactAttribute : PlatformFactAttribute
    {
        public IgnoreOnPlatformFactAttribute(params OSPlatforms[] oSPlatforms)
        {
            if (oSPlatforms.Any(oSPlatform =>
            {
                var osPlatformValue = GetOSPlatform(oSPlatform);
                if (osPlatformValue is null) return false;
                return RuntimeInformation.IsOSPlatform(osPlatformValue.Value);
            }))
            {
                Skip = $"Ignored on OsPlatform incompatibility";
            }
        }
        
    }
}
