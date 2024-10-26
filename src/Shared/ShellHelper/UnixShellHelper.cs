using System.Text;

using Commandy.Abstractions;

namespace Commandy.Internals.ShellHelper
{
    internal class UnixShellHelper : IShellHelper
    {
        public string GetArguments(ICommand command)
        {
            var commandBuilder = new StringBuilder();
            if (command.Options.UseShell)
            {
                commandBuilder.Append(Constants.SHELL_ARG);
                commandBuilder.Append(Internals.Constants.WHITE_SPACE);
                commandBuilder.Append(Internals.Constants.DOUBLE_QUATES);
                commandBuilder.Append(command.CommandText);
                commandBuilder.Append(ShellHelperBase.GetArguments(command.Options.Arguments));
                commandBuilder.Append(Internals.Constants.DOUBLE_QUATES);
            }
            else
            {
                commandBuilder.Append(ShellHelperBase.GetArguments(command.Options.Arguments));
            }

            return commandBuilder.ToString();
        }

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
            }

            return commandBuilder.ToString();
        }

        public string GetShell()
        {
            return System.IO.File.Exists(Constants.BASH) ? Constants.BASH : Constants.SH;
        }

        public string GetShellArgument()
        {
            return Constants.SHELL_ARG;
        }

        public string GetShellFlag()
        {
            return Constants.SHELL_FLAG;
        }
    }
}