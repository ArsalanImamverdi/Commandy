# Commandy

Commandy is a powerful, cross-platform .NET library for executing shell commands and processes. It provides a simple, intuitive API for running commands on both Windows and Linux environments, with support for synchronous and asynchronous execution.

## Features

- **Cross-Platform Support**: Works seamlessly on Windows and Linux.
- **Flexible Execution**: Support for both synchronous and asynchronous command execution.
- **Shell and Non-Shell Execution**: Run commands with or without using the system shell.
- **Configurable Options**: Customize working directory, environment variables, and command arguments.
- **Event-Driven Output**: Real-time access to command output and error streams via events.
- **Cancellation Support**: Cancel long-running commands using cancellation tokens.
- **Extensible Design**: Easy to extend for custom requirements.
- **Piping Support**: Chain commands together by piping output from one command to another.
- **Timeout Handling**: Set timeouts for command execution to prevent hanging processes.
- **Environment Variable Management**: Easily set and manage environment variables for commands.
- **Working Directory Configuration**: Specify the working directory for command execution.
- **Argument Builder**: Fluent interface for building complex command arguments.
- **Dependency Injection Support**: Seamlessly integrate with .NET dependency injection containers.


## Installation

Install Commandy via NuGet:

```
dotnet add package Commandy
```

## Quick Start

Here's a simple example to get you started:

```csharp
using Commandy;

// Create and execute a command
var result = Command.CreateCommand("echo Hello, World!")
    .Execute();

Console.WriteLine($"Output: {result.Output}");
Console.WriteLine($"Exit Code: {result.ExitCode}");
```

## Advanced Usage

### Asynchronous Execution

```csharp
var command = Command.CreateCommand("long-running-process");
var result = await command.ExecuteAsync();
```

### Custom Options

```csharp
var command = CommandProvider.CreateCommand("git", opt => opt
    .UseShell(false)
    .AddEnvironmentVariable("GIT_CREDS_PATH", "/path/to/cred/file")
    .AddArgument("clone")
    .AddArgument("https://github.com/example/repo.git")
    .WorkingDirectory("/path/to/directory")
    .Timeout(TimeSpan.FromMinutes(5))
);
```

### Event Handling

```csharp
var command = Command.CreateCommand("dir");
command.OnDataReceived += (sender, args) => Console.WriteLine($"Received: {args.Data}");
command.OnErrorReceived += (sender, args) => Console.WriteLine($"Error: {args.Error}");
await command.ExecuteAsync();
```

### Command Piping

```csharp
var grepCommand = CommandProvider.CreateCommand("grep error");
var command = CommandProvider.CreateCommand("cat log.txt", opt => opt
    .PipeTo(grepCommand)
);
var result = command.Execute();
```

### Using with Dependency Injection

```csharp
services.AddCommandy();

// In your code:
public class MyService
{
    private readonly ICommandProvider _commandProvider;

    public MyService(ICommandProvider commandProvider)
    {
        _commandProvider = commandProvider;
    }

    public void RunCommand()
    {
        var command = _commandProvider.CreateCommand("echo Hello from DI!");
        var result = command.Execute();
        Console.WriteLine(result.Output);
    }
}
```

### Documentation

For full documentation, please visit our [Wiki](/wiki).

### License

Commandy is licensed under the MIT License.
