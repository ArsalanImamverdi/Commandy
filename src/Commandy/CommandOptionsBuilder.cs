﻿namespace Commandy
{
    public class CommandOptionsBuilder
    {
        private readonly CommandOptions options;
        public CommandOptionsBuilder()
        {
            options = new CommandOptions();
        }
        public CommandOptionsBuilder UseShell(bool useShell = true)
        {
            options.UseShell = useShell;
            return this;
        }
        public CommandOptionsBuilder AddArgument(string argument)
        {
            return AddArgument(argument, null);
        }
        public CommandOptionsBuilder AddArgument(string argument, string value)
        {
            options.Arguments.Add(new CommandArguments() { Flag = argument, Value = value });
            return this;
        }
        public CommandOptionsBuilder AddEnvironmentVariable(string key, string value)
        {
            options.EnvironmentVariables.Add(key, value);
            return this;
        }
        public CommandOptionsBuilder WorkingDirectory(string workingDirectory)
        {
            options.WorkingDirectory = workingDirectory;
            return this;
        }
        public CommandOptions Build()
        {
            return options;
        }
    }
}
