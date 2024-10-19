using System.Diagnostics;

namespace Commandy.Tests
{
    public class WindowsCommandTests
    {
        [Fact]
        public async Task RunCommand_WithShell_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = Command.CreateCommand("dir");
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [Fact]
        public void RunCommand_WithoutShell_NoneAsync_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = Command.CreateCommand("git", opt => opt.UseShell(false).AddArgument("--version"));
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = command.Execute();

            Assert.Equal(0, result.ExitCode);
        }

        [Fact]
        public void RunCommand_WithShell_WhenCancel_ShouldExitCodeBeLessThanZeroAndContainsContentBeforExit()
        {
            var command = Command.CreateCommand("echo.bat");
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(6000);

            var result = command.Execute(cancellation.Token);

            Assert.Equal(-1, result.ExitCode);
            Assert.Contains("First", result.Output);
            Assert.Contains("Second", result.Output);
            Assert.DoesNotContain("Third", result.Output);
        }

        [Fact]
        public void RunCommand_WithShell_WhenHasError_ShouldExitCodeBeGreaterThanZeroAndHasErrorAndNoOutput()
        {
            var command = Command.CreateCommand("something-cause-error");

            var result = command.Execute();
            Assert.True(result.ExitCode > 0);
            Assert.Empty(result.Output);
            Assert.NotEmpty(result.Error);
        }
        [Fact]
        public void Test()
        {
            Debugger.Launch();
            string imageFileName = Guid.NewGuid().ToString();
            var command = Commandy.Command.CreateCommand("ffprobe", opt => opt.AddArgument("-v", "error").AddArgument("-show_error").AddArgument("-show_streams").AddArgument(imageFileName));
            var result = command.Execute();
        }
    }
}
