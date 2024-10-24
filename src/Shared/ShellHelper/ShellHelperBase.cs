using System.Collections.Generic;
using System.Text;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class ShellHelperBase
    {
        public static string GetWindowsArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return BuildArguments(commandArguments);
        }

        public static string GetUnixArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            return BuildArguments(commandArguments);
        }

        private static string BuildArguments(IReadOnlyCollection<ICommandArgument> commandArguments)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Internals.Constants.WHITE_SPACE);

            foreach (ICommandArgument argument in commandArguments)
            {

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
    }
}
