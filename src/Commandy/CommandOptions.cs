using System.Collections.Generic;
using System.Linq;

namespace Commandy
{
    public class CommandOptions
    {
        public bool UseShell { get; set; }
        public string Command { get; set; }
        public List<CommandArguments> Arguments { get; set; } = new List<CommandArguments>();

        internal string GetArguments()
        {
            return Arguments.Select(arg => $"{arg.GetArgument()}").DefaultIfEmpty("").Aggregate((f, s) => $"{f} {s}");
        }
    }
}
