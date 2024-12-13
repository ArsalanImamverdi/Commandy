using Commandy.Abstractions;
using Commandy.DependencyInjection;
using Commandy.Tests.Attributes;

using Microsoft.Extensions.DependencyInjection;

namespace Commandy.Tests
{
    public class LinuxCommandTests
    {
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenPiped_ShouldExitCodeBeZero()
        {
            var pipeCommand = CommandProvider.CreateCommand("grep", opt => opt.AddArgument("total"));
            var command = CommandProvider.CreateCommand("ls", opt => opt.AddArgument("-la").PipeTo(pipeCommand));

            var result = command.Execute();
            Assert.True(result.ExitCode == 0, result.Output);
            Assert.Contains("total", result.Output);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public async Task RunCommand_WithShell_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("ls", opt => opt);
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [RunOnPlatformFact(OSPlatforms.Linux)]
        public async Task RunCommand_WithShell_WhenAddArgument_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("echo $TEST_ARG", opt => opt.AddEnvironmentVariable("TEST_ARG", "TEST_VALUE"));
            await Task.Delay(1000);
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("TEST_VALUE", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenCancel_ShouldExitCodeBeLessThanZeroAndContainsContentBeforeExit()
        {
            var contentFile = File.ReadAllText("echo.sh");
            contentFile = contentFile.Replace('\r', ' ');
            File.WriteAllText("echo.sh", contentFile);
            var chmod = CommandProvider.CreateCommand("chmod", opt => opt.AddArgument("+x").AddArgument("echo.sh"));
            var chmodResult = chmod.Execute();
            Assert.Equal(0, chmodResult.ExitCode);

            var command = CommandProvider.CreateCommand("./echo.sh", opt => opt);
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(6000);

            var result = command.Execute(cancellation.Token);

            Assert.True(result.ExitCode > 0);
            Assert.Contains("First", result.Output);
            Assert.Contains("Second", result.Output);
            Assert.DoesNotContain("Third", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Linux)]
        public async Task RunCommand_WithShell_UseDependencyInjection_WhenSuccess_ShouldExitCodeBeZero()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandy();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var commandProvider = serviceProvider.GetRequiredService<ICommandProvider>();
            var command = commandProvider.CreateCommand("ls", opt => opt);

            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenChained_ShouldExitCodeBeZero()
        {
            var chainCommand = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World!"));
            var chainCommand2 = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World! from other chained!"));
            var command = CommandProvider.CreateCommand("errory-command", opt => opt.ChainTo(chainCommand2, CommandChainType.Or).ChainTo(chainCommand, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Hello, World! from other chained!\n", result.Output);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenDoubleChained_ShouldExitCodeBeZero()
        {
            var echoCommand = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World!"));
            var exitCommand = CommandProvider.CreateCommand("exit 1", opt => opt.ChainTo(echoCommand, CommandChainType.And));
            var command = CommandProvider.CreateCommand("errory-command", opt => opt.ChainTo(echoCommand, CommandChainType.Or).ChainTo(exitCommand, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(1, result.ExitCode);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenDoubleChainedAndPiped_ShouldExitCodeBeZero()
        {
            var pipeCommand = CommandProvider.CreateCommand("grep", opt => opt.AddArgument("World"));
            var echoCommand = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World!").PipeTo(pipeCommand));
            var r = echoCommand.Execute();
            var echoCommandMore = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World! from other chained!").ChainTo(echoCommand, CommandChainType.And));
            var command = CommandProvider.CreateCommand("errory-command", opt => opt.ChainTo(echoCommand, CommandChainType.Or).ChainTo(echoCommandMore, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Hello, World!\n", result.Output);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenChainedWithPreserveLog_ShouldExitCodeBeZero()
        {
            var echoCommand = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World!"));
            var echoCommandMore = CommandProvider.CreateCommand("echo", opt => opt.AddArgument("Hello, World! from other chained!"));
            var command = CommandProvider.CreateCommand("errory-command", opt => opt.ChainTo(echoCommand, CommandChainType.Or).ChainTo(echoCommandMore, CommandChainType.And));
            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.NotEqual("Hello, World! from other chained!\n", result.Output);
            Assert.Contains("Hello, World! from other chained!\n", result.Output);
        }
    }
}
