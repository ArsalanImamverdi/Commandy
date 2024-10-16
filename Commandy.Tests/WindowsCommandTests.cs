using Commandy.Internals;

namespace Commandy.Tests
{
    public class WindowsCommandTests
    {
        [Fact]
        public void RunCommand_WithShell_WhenSuccess_ShouldSuccess()
        {
            var command = Command.CreateCommand("dir");
            var result = command.Execute();

            Assert.Equal(0, result.ExitCode);
        }


    }
}
