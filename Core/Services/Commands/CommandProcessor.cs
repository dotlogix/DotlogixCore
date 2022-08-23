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
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Services.Commands {
    public sealed class CommandProcessor : ICommandProcessor {
        private static readonly Regex ArgumentRegex = new("\\s(?:(?<name>\\w+)\\s*[=:])?\\s*(?:(?:\\\"(?<value>[^\\\"]+)\\\")|(?<value>[^\\\"\\s]+))");
        private readonly IReadOnlyDictionary<string, IConsoleCommand> _commands;
        private readonly IServiceProvider _serviceProvider;

        public CommandProcessor(IServiceProvider serviceProvider, IEnumerable<IConsoleCommand> commands) {
            _serviceProvider = serviceProvider;
            _commands = commands.ToDictionary(
                c => c.Name,
                StringComparer.OrdinalIgnoreCase
            );
        }

        public async Task<int?> ProcessAsync(string text) {
            var commandName = text.SubstringUntil(' ');
            var namedArgs = new Dictionary<string, string>();
            var unnamedArgs = new List<string>();
            if(commandName.Length < text.Length) {
                var argsMatches = ArgumentRegex.Matches(text, commandName.Length);

                foreach(Match match in argsMatches) {
                    if(!match.Success) {
                        continue;
                    }

                    var name = match.Groups["name"].GetValueOrDefault();
                    var value = match.Groups["value"].GetValueOrDefault();

                    if(name == null) {
                        unnamedArgs.Add(value);
                    } else {
                        namedArgs[name] = value;
                    }
                }
            }

            var command = _commands.GetValueOrDefault(commandName);
            if(command is null) {
                Console.WriteLine($"Command {commandName} could not be found.\nPossible commands:");
                CommandBuilderExtensions.Describe(_commands.Values);
                return null;
            }

            var args = new CommandArgs(namedArgs, unnamedArgs);
            var context = new CommandContext(_serviceProvider, _commands, command, args);
            try {
                await command.ExecuteAsync(context);
            } catch(Exception e) {
                context.SetException(e);
            }

            if(context.HasResult == false) {
                return context.ExitCode;
            }

            if(context.Exception is not null) {
                Console.WriteLine($"Invalid usage of command {commandName}");
                CommandBuilderExtensions.Help(command);
                return null;
            } 
        
            if(context.Result is not null) {
                Console.WriteLine(context.Result);
            }
            return context.ExitCode;
        }
    }
}
