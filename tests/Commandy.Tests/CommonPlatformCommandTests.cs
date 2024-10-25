namespace Commandy.Tests
{
    public class CommonPlatformCommandTests
    {
        [Fact]
        public void RunCommand_WithShell_WhenHasError_ShouldExitCodeBeGreaterThanZeroAndHasErrorAndNoOutput()
        {
            var command = CommandProvider.CreateCommand("something-cause-error", opt => opt.UseShell());

            var result = command.Execute();
            Assert.True(result.ExitCode > 0);
            Assert.Empty(result.Output);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public void RunCommand_WithoutShell_NoneAsync_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("git", opt => opt.UseShell(false).AddArgument("--version"));
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = command.Execute();

            Assert.Equal(0, result.ExitCode);
        }
    }
}
