using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Services.Commands; 

public static class CommandBuilderExtensions {
    public static ICommandProcessorBuilder UseDefaultCommands(this ICommandProcessorBuilder builder) {
        return builder
           .UseExitCommand()
           .UseHelpCommand()
           .UseDescribeCommand()
           .UseClearCommand();
    }
    
    public static ICommandProcessorBuilder UseExitCommand(this ICommandProcessorBuilder builder) {
        return builder
           .UseCommand(command => command
               .UseName("exit")
               .UseDescription("Stops the execution of this application")
               .UseCallback(context => context.SetExitCode(0))
            );
    }

    public static ICommandProcessorBuilder UseHelpCommand(this ICommandProcessorBuilder builder) {
        return builder
           .UseCommand(command => command
               .UseName("help")
               .UseDescription("Prints the description and usage information for commands")
               .UseHelpText("help\nhelp [command]")
               .UseCallback(context => {
                    var args = context.Arguments.Unnamed;
                    if(args is { Count: 1 }) {
                        Help(context.AvailableCommands.GetValueOrDefault(args[0]));
                    } else {
                        Help(context.AvailableCommands.Values);
                    }
                })
            );
    }

    public static ICommandProcessorBuilder UseDescribeCommand(this ICommandProcessorBuilder builder) {
        return builder
           .UseCommand(command => command
               .UseName("describe")
               .UseDescription("Prints the description for commands")
               .UseHelpText("describe\ndescribe [command]")
               .UseCallback(context => {
                    var args = context.Arguments.Unnamed;
                    if(args is { Count: 1 }) {
                        Describe(context.AvailableCommands.GetValueOrDefault(args[0]));
                    } else {
                        Describe(context.AvailableCommands.Values);
                    }
                })
            );
    }

    public static ICommandProcessorBuilder UseClearCommand(this ICommandProcessorBuilder builder) {
        return builder
           .UseCommand(command => command
               .UseName("clear")
               .UseHelpText("Clears the current console output")
               .UseCallback(_ => Console.Clear())
            );
    }

    public static void Describe(IConsoleCommand command) {
        Console.WriteLine($"{command.Name}: {command.Description}");
    }

    public static void Describe(IEnumerable<IConsoleCommand> commands) {
        foreach(var command in commands.OrderBy(c => c.Name)) {
            Describe(command);
        }
    }

    public static void Help(IConsoleCommand command) {
        Describe(command);
        if(command.HelpText is not null) {
            Console.WriteLine(command.HelpText);
        }
    }

    public static void Help(IEnumerable<IConsoleCommand> commands) {
        foreach(var command in commands.OrderBy(c => c.Name)) {
            Help(command);
        }
    }
}