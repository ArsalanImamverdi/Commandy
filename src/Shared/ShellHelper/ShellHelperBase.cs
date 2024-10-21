using System.Collections.Generic;
using System.Text;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class ShellHelperBase
    {
        public static string GetWindowsArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return BuildArguments(commandArguments, Constants.CMD_FLAG, false);
        }

        public static string GetUnixArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return BuildArguments(commandArguments, Constants.SHELL_FLAG, true);
        }

        private static string BuildArguments(IReadOnlyCollection<ICommandArgument> commandArguments, string flag, bool isUnix)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Internals.Constants.WHITE_SPACE);

            foreach (ICommandArgument argument in commandArguments)
            {
                if (ShouldAddFlag(argument, isUnix))
                {
                    stringBuilder.Append(flag);
                }

                stringBuilder.Append(argument.Argument);

                if (!string.IsNullOrEmpty(argument.Value))
                {
                    stringBuilder.Append(Internals.Constants.WHITE_SPACE);
                    stringBuilder.Append(argument.Value);
                }

                stringBuilder.Append(Internals.Constants.WHITE_SPACE);
            }

            return stringBuilder.ToString();
        }

        private static bool ShouldAddFlag(ICommandArgument argument, bool isUnix)
        {
            if (isUnix)
            {
                return !argument.Argument.StartsWith(Constants.SHELL_FLAG)
                       && !argument.Argument.StartsWith(Constants.CMD_FLAG)
                       && !string.IsNullOrEmpty(argument.Value)
                       && (argument.Argument.Length == 1 || argument.Argument.Length > 1);
            }
            else
            {
                return !argument.Argument.StartsWith(Constants.CMD_FLAG)
                       && !string.IsNullOrEmpty(argument.Value);
            }
        }
    }
}
