using System.Text;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class WindowsShellHelper : IShellHelper
    {
        public string GetExecutable(ICommand command)
        {
            var commandBuilder = new StringBuilder();
            if (command.Options.UseShell)
            {
                commandBuilder.Append(GetShell());
            }
            else
            {
                commandBuilder.Append(command.CommandText);
                commandBuilder.Append(Internals.Constants.WHITE_SPACE);
            }

            return commandBuilder.ToString();
        }

        public string GetArguments(ICommand command)
        {
            var commandBuilder = new StringBuilder();
            if (command.Options.UseShell)
            {
                commandBuilder.Append(Constants.CMD_ARG);
                commandBuilder.Append(Internals.Constants.WHITE_SPACE);
                commandBuilder.Append(command.CommandText);
                commandBuilder.Append(Internals.Constants.WHITE_SPACE);
                commandBuilder.Append(ShellHelperBase.GetArguments(command.Options.Arguments));
            }
            else
            {
                commandBuilder.Append(ShellHelperBase.GetArguments(command.Options.Arguments));
            }

            return commandBuilder.ToString();
        }

        public string GetShell()
        {
            return Constants.CMD;
        }

        public string GetShellArgument()
        {
            return Constants.CMD_ARG;
        }

        public string GetShellFlag()
        {
            return Constants.CMD_FLAG;
        }
    }
}
