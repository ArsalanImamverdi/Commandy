using Commandy.Abstractions;
using Commandy.DependencyInjection;
using Commandy.Tests.Attributes;

using Microsoft.Extensions.DependencyInjection;

namespace Commandy.Tests
{
    public class WindowsCommandTests
    {
        [RunOnPlatformFact(OSPlatforms.Windows)]
        public async Task RunCommand_WithoutShell_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("dir");
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithoutShell_NoneAsync_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("git", opt => opt.UseShell(false).AddArgument("--version"));
            var evnt = string.Empty;
            command.OnDataReceived += (s, l) =>
            {
                evnt = l.Data;
            };
            var result = command.Execute();

            Assert.Equal(0, result.ExitCode);
            Assert.NotEmpty(evnt);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenCancel_ShouldExitCodeBeLessThanZeroAndContainsContentBeforeExit()
        {
            var command = CommandProvider.CreateCommand("echo.bat", opt => opt.UseShell());
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(6000);

            var result = command.Execute(cancellation.Token);

            Assert.Equal(-1, result.ExitCode);
            Assert.Contains("First", result.Output);
            Assert.Contains("Second", result.Output);
            Assert.DoesNotContain("Third", result.Output);
        }

        private void Command_OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenHasError_ShouldExitCodeBeGreaterThanZeroAndHasErrorAndNoOutput()
        {
            var command = CommandProvider.CreateCommand("something-cause-error", opt => opt.UseShell());

            var result = command.Execute();
            Assert.Equal(1, result.ExitCode);
            Assert.Empty(result.Output);
            Assert.NotEmpty(result.Error);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenPiped_ShouldExitCodeBeZero()
        {
            var pipeCommand = CommandProvider.CreateCommand("findstr", opt => opt.UseShell().AddArgument("Number"));
            var pipeAnotherCommand = CommandProvider.CreateCommand("findstr", opt => opt.UseShell().AddArgument("Serial"));
            var command = CommandProvider.CreateCommand("dir", opt => opt.UseShell().PipeTo(pipeAnotherCommand).PipeTo(pipeCommand));

            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("ial", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenChained_ShouldExitCodeBeZero()
        {
            var chainCommand = CommandProvider.CreateCommand("echo", opt => opt.UseShell().AddArgument("Hello, World!"));
            var chainCommand2 = CommandProvider.CreateCommand("echo", opt => opt.UseShell().AddArgument("Hello, World! from other chained!"));
            var command = CommandProvider.CreateCommand("errory-command", opt => opt.UseShell().ChainTo(chainCommand, CommandChainType.Or).ChainTo(chainCommand2, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains(" Hello, World! from other chained! \r\n", result.Output);
        }
        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenChainedWithPreserveLog_ShouldExitCodeBeZero()
        {
            var chainCommand = CommandProvider.CreateCommand("findstr", opt => opt.UseShell().AddArgument("Serial"));
            var chainCommand1 = CommandProvider.CreateCommand("dir", opt => opt.UseShell().PipeTo(chainCommand));
            var chainCommand2 = CommandProvider.CreateCommand("echo", opt => opt.UseShell().AddArgument("Hello, World! from other chained!"));
            var command = CommandProvider.CreateCommand("error-command", opt => opt.UseShell().ChainTo(chainCommand1, CommandChainType.Or).ChainTo(chainCommand2, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.NotEqual(" Hello, World! from other chained! \r\n", result.Output);
            Assert.Contains(" Hello, World! from other chained! \r\n", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public async Task RunCommand_WithShell_UseDependencyInjection_WhenSuccess_ShouldExitCodeBeZero()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandy();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var commandProvider = serviceProvider.GetRequiredService<ICommandProvider>();
            var command = commandProvider.CreateCommand("dir", opt => opt.UseShell());
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public async Task RunCommand_WithShell_WhenTimeout_ShouldExitCodeBeGreaterThanZero()
        {
            var command = CommandProvider.CreateCommand("echo.bat", opt => opt.UseShell().Timeout(TimeSpan.FromSeconds(1)));
            var result = await command.ExecuteAsync();

            Assert.True(result.ExitCode < 0);
        }
    }
}
