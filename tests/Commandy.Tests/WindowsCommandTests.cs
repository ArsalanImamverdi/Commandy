﻿using Commandy.Abstractions;
using Commandy.DependencyInjection;
using Commandy.Tests.Attributes;

using Microsoft.Extensions.DependencyInjection;

namespace Commandy.Tests
{
    public class WindowsCommandTests
    {
        [RunOnPlatformFact(OSPlatforms.Windows)]
        public async Task RunCommand_WithShell_WhenSuccess_ShouldExitCodeBeZero()
        {
            var command = CommandProvider.CreateCommand("dir", opt => opt.UseShell());
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenCancel_ShouldExitCodeBeLessThanZeroAndContainsContentBeforeExit()
        {

            var command = CommandProvider.CreateCommand("echo.bat");
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(6000);

            var result = command.Execute(cancellation.Token);

            Assert.Equal(-1, result.ExitCode);
            Assert.Contains("First", result.Output);
            Assert.Contains("Second", result.Output);
            Assert.DoesNotContain("Third", result.Output);
        }


        [RunOnPlatformFact(OSPlatforms.Windows)]
        public void RunCommand_WithShell_WhenPiped_ShouldExitCodeBeZero()
        {
            var pipeCommand = CommandProvider.CreateCommand("findstr", opt => opt.UseShell().AddArgument("Serial"));
            var command = CommandProvider.CreateCommand("dir", opt => opt.UseShell().PipeTo(pipeCommand));

            var result = command.Execute();
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Serial", result.Output);
        }

        [RunOnPlatformFact(OSPlatforms.Windows)]
        public async Task RunCommand_WithShell_UseDependencyInjection_WhenSuccess_ShouldExitCodeBeZero()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommandy();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var commandProvider = serviceProvider.GetRequiredService<ICommandProvider>();
            var command = commandProvider.CreateCommand("dir", opt => opt.UseShell());
            command.OnDataReceived += (sender, args) =>
            {
                Console.WriteLine(args.Data);
            };
            var result = await command.ExecuteAsync();

            Assert.Equal(0, result.ExitCode);
        }
    }
}
