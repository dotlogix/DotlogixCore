// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  CommandProcessor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 17.04.2022 17:24
// LastEdited:  17.04.2022 17:24
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Services.Commands;

public class CommandProcessor : ICommandProcessor {
    private static readonly Regex ArgumentRegex = new("\\s(?:(?<name>\\w+)\\s*[=:])?\\s*(?:(?:\\\"(?<value>[^\\\"]+)\\\")|(?<value>[^\\\"\\s]+))");
    public IKeyedCollection<string, IConsoleCommand> Commands { get; } = new KeyedCollection<string, IConsoleCommand>(c => c.Name, StringComparer.OrdinalIgnoreCase);

    public CommandProcessor() {
        Commands.AddOrUpdate(new [] {
            new LambdaConsoleCommand(
                "help",
                "Prints the description and usage information for commands",
                "help\nhelp [command]",
                OnCommand_Help
            ),
            new LambdaConsoleCommand(
                "describe",
                "Prints the description for commands",
                "describe\ndescribe [command]",
                OnCommand_Describe
            ),
            new LambdaConsoleCommand(
                "clear",
                "Clears the current console output",
                OnCommand_Clear
            )
        });
    }

    public async Task ProcessAsync(string text) {
        var commandName = text.SubstringUntil(' ');
        var namedArgs = new Dictionary<string, string>();
        var unnamedArgs = new List<string>();
        if(commandName.Length < text.Length) {
            var argsMatches = ArgumentRegex.Matches(text, commandName.Length);

            foreach(Match match in argsMatches) {
                if(!match.Success)
                    continue;

                var name = match.Groups["name"]
                   .GetValueOrDefault();
                var value = match.Groups["value"]
                   .GetValueOrDefault();

                if(name == null)
                    unnamedArgs.Add(value);
                else
                    namedArgs[name] = value;
            }
        }

        var command = Commands.Get(commandName);
        if(command is null) {
            Console.WriteLine($"Command {commandName} could not be found.\nPossible commands:");
            Describe();
            return;
        }
        
        var args = new CommandArgs(namedArgs, unnamedArgs);
        try {
            await command.ExecuteAsync(args);
        } catch {
            Console.WriteLine($"Invalid usage of command {commandName}");
            Help(command);
        }
    }
    
    private void OnCommand_Describe(CommandArgs args) {
        if(args.Unnamed is { Count: 1 }) {
            Describe(Commands.Get(args.Unnamed[0]));
        } else {
            Describe();
        }
    }
    private void OnCommand_Help(CommandArgs args) {
        if(args.Unnamed is { Count: 1 }) {
            Help(Commands.Get(args.Unnamed[0]));
        } else {
            Help();
        }
    }
    private void OnCommand_Clear(CommandArgs args) {
        Console.Clear();
    }

    private void Describe() {
        foreach(var command in Commands.OrderBy(c=>c.Name)) {
            Describe(command);
        }
    }
    
    private void Describe(IConsoleCommand command) {
        Console.WriteLine($"{command.Name}: {command.Description}");
    }
    
    private void Help() {
        foreach(var command in Commands.OrderBy(c=>c.Name)) {
            Help(command);
        }
    }
    
    private void Help(IConsoleCommand command) {
        Describe(command);
        if(command.HelpText is not null) {
            Console.WriteLine(command.HelpText);
        }
    }
}
