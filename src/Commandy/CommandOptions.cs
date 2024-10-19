using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Commandy
{
    public class CommandOptions
    {
        internal bool UseShell { get; set; }
        internal string Command { get; set; }
        internal List<CommandArguments> Arguments { get; set; } = new List<CommandArguments>();
        internal string WorkingDirectory { get; set; }
        internal StringDictionary EnvironmentVariables { get; set; }
        internal TimeSpan Timeout { get; set; }
        internal string GetArguments()
        {
            return string.Join(" ", Arguments.Select(arg => arg.GetArgument()));
        }
    }
}
