using Commandy.Abstractions;
using Commandy.Tests.Attributes;
using Commandy.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Commandy.Tests
{
    public class LinuxCommandTests
    {
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenPiped_ShouldExitCodeBeZero()
        {
            var pipeCommand = CommandProvider.CreateCommand("grep", opt => opt.UseShell().AddArgument("total"));
            var command = CommandProvider.CreateCommand("ls", opt => opt.UseShell().AddArgument("-l").AddArgument("-a").PipeTo(pipeCommand));

            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("total", result.Output);
        }
        [RunOnPlatformFact(OSPlatforms.Linux)]
        public async Task RunCommand_WithShell_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("ls", opt => opt.UseShell());
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [RunOnPlatformFact(OSPlatforms.Linux)]
        public async Task RunCommand_WithShell_WhenAddArgument_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("env", opt => opt.UseShell(false).AddEnvironmentVariable("TEST_ARG", "TEST_VALUE"));
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
            Assert.Contains("TEST_VALUE", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Linux)]
        public void RunCommand_WithShell_WhenCancel_ShouldExitCodeBeLessThanZeroAndContainsContentBeforeExit()
        {
            var chmod = CommandProvider.CreateCommand("chmod", opt => opt.UseShell().AddArgument("+x").AddArgument("echo.sh"));
            var chmodResult = chmod.Execute();
            Assert.Equal(0, chmodResult.ExitCode);

            var command = CommandProvider.CreateCommand("./echo.sh", opt => opt.UseShell());
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
            var command = commandProvider.CreateCommand("ls", opt => opt.UseShell());
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }
    }
}
