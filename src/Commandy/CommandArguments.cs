namespace Commandy
{
    public class CommandArguments
    {
        public string Flag { get; set; }
        public string Value { get; set; }

        internal string GetArgument()
        {
            var argument = Flag;
            if (!string.IsNullOrEmpty(Value))
                argument = string.Join(" ", new string[] { argument, Value });
            return argument;
        }
    }
}
