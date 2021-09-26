// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WindowsService.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotLogix.Core.Collections;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Services {
    /// <summary>
    ///     A base class for service like applications
    /// </summary>
    public class Service {
        /// <summary>
        ///     The application mode
        /// </summary>
        public ApplicationMode Mode { get; set; }

        /// <summary>
        ///     The executable path
        /// </summary>
        public string ProgramExecutable { get; }

        /// <summary>
        ///     The log directory
        /// </summary>
        public string LogDirectory { get; }

        /// <summary>
        ///     The service name
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        ///     A collection of commands for user interactive mode
        /// </summary>
        public KeyedCollection<string, ConsoleCommand> Commands { get; } = new KeyedCollection<string, ConsoleCommand>(c => c.Name);

        /// <summary>
        ///     Creates a new instance of <see cref="Service" />
        /// </summary>
        public Service(string serviceName, string logDirectory) {
            ProgramExecutable = Assembly.GetEntryAssembly()?.Location;
            ServiceName = serviceName;
            LogDirectory = logDirectory;

            Commands.AddOrUpdate(new LambdaConsoleCommand("exit", "exit", OnCommand_Exit));
            Commands.AddOrUpdate(new LambdaConsoleCommand("help", "help", OnCommand_Help));
            Commands.AddOrUpdate(new LambdaConsoleCommand("clear", "clear", OnCommand_Clear));
        }

        /// <summary>
        ///     A callback to start the service
        /// </summary>
        protected virtual void OnStart(string[] args) { }

        /// <summary>
        ///     A callback to stop the service
        /// </summary>
        protected virtual void OnStop() {
            BroadcastLogger.Instance.Shutdown();
        }

        /// <summary>
        ///     Start the service loop
        /// </summary>
        public virtual void Run(string[] args) {
            OnConfiguration(args);

            if(Mode == ApplicationMode.UserInteractive)
                UserInteractiveStartup(args);
            else
                ServiceStartup(args);
        }

        /// <summary>
        ///     Create the required loggers
        /// </summary>
        protected virtual void InitializeLoggers(string[] args) {
            var loggers = new List<ILogger>();
            if(LogDirectory != null)
                loggers.Add(new FileLogger { LogFile = Path.Combine(LogDirectory, $"{DateTime.UtcNow:s}.log") });

            if(Mode == ApplicationMode.UserInteractive && args.Contains("--no-console-log") == false)
                loggers.Add(new ConsoleLogger());

            if(loggers.Count > 0) {
                BroadcastLogger.Instance.AttachLogger(loggers);
                BroadcastLogger.Instance.Initialize();
            }
        }

        /// <summary>
        ///     A callback to configure the service
        /// </summary>
        protected virtual void OnConfiguration(string[] args) {
            if(Environment.UserInteractive == false || args.Contains("--service"))
                Mode = ApplicationMode.Service;
            else
                Mode = ApplicationMode.UserInteractive;
            
            InitializeLoggers(args);
        }

        protected virtual void UserInteractiveStartup(string[] args) {
            if (args.Length > 0) {
                switch (args[0].ToLower()) {
                    case "--install":
                    case "--i":
                        if (OnCommand_Install()) {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Service installation successful");
                        } else {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Service installation failed");
                        }

                        Console.ResetColor();
                        Console.WriteLine("Press Enter to exit");
                        Console.ReadLine();
                        return;
                    case "--uninstall":
                    case "--u":
                        if (OnCommand_Uninstall()) {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Service uninstallation successful");
                        } else {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Service uninstallation failed");
                        }

                        Console.ResetColor();
                        Console.WriteLine("Press Enter to exit");
                        Console.ReadLine();
                        return;
                }
            }

            try {
                OnStart(args);
                Environment.ExitCode = ProcessUserInput();
            } catch (Exception e) {
                Log.Critical(e);
                Environment.ExitCode = e.HResult; // error code
            } finally {
                OnStop();
            }
        }

        protected virtual void ServiceStartup(string[] args) {
            try {
                OnStart(args);
                Console.ReadLine();

                Environment.ExitCode = 0; // success
            } catch (Exception e) {
                Log.Critical(e);
                Environment.ExitCode = e.HResult; // error code
            } finally {
                OnStop();
            }
        }

        protected virtual int ProcessUserInput() {
            var commandResult = ConsoleCommandResult.CommandCompleted;
            var commandParserRegex = new Regex("\\s(?:(?<name>\\w+)\\s*[=:])?\\s*(?:(?:\\\"(?<value>[^\\\"]+)\\\")|(?<value>[^\\\"\\s]+))");
            do {
                var line = Console.ReadLine();
                if(line == null)
                    continue;

                var commandName = line.SubstringUntil(' ');

                var commandArgs = new Dictionary<string, string>();
                var unnamedCommandArgs = new List<string>();
                if(commandName.Length < line.Length) {
                    var argsMatches = commandParserRegex.Matches(line, commandName.Length);

                    foreach(Match match in argsMatches) {
                        if(!match.Success)
                            continue;

                        var name = match.Groups["name"]
                                        .GetValueOrDefault();
                        var value = match.Groups["value"]
                                         .GetValueOrDefault();

                        if(name == null)
                            unnamedCommandArgs.Add(value);
                        else
                            commandArgs[name] = value;
                    }
                }

                commandResult = OnCommand(commandName, new CommandArgs(commandArgs, unnamedCommandArgs));
            } while(commandResult.Exit == false);

            return commandResult.ExitCode;
        }

        #region OnCommand
        /// <summary>
        ///     A callback to handle a user command
        /// </summary>
        protected virtual ConsoleCommandResult OnCommand(string command, CommandArgs args) {
            if(Commands.TryGetValue(command, out var cmd)) {
                if((args.Unnamed.Count == 1) && (args.Unnamed[0] == "help")) {
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return ConsoleCommandResult.CommandCompleted;
                }
                try {
                    var commandResult = cmd.OnExecute(args);
                    if(commandResult.Success)
                        return commandResult;

                    Console.WriteLine("Invalid usage of command " + cmd.Name);
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return ConsoleCommandResult.CommandFailed;

                } catch (Exception e) {
                    Log.Error(e);
                    return ConsoleCommandResult.CommandFailed;
                }
            }

            Console.WriteLine("Command could not be found possible commands are");
            OnCommand_Help();
            return ConsoleCommandResult.CommandCompleted;
        }

        protected virtual ConsoleCommandResult OnCommand_Exit() {
            return ConsoleCommandResult.ExitNoError;
        }

        protected virtual void OnCommand_Help() {
            foreach(var consoleCommand in Commands.OrderBy(c => c.Name))
                Console.WriteLine(consoleCommand.Description);
        }

        protected virtual void OnCommand_Clear() {
            Console.Clear();
        }

        /// <summary>
        ///     A callback to install the service
        /// </summary>
        protected virtual bool OnCommand_Install() {
            return false;
        }


        /// <summary>
        ///     A callback to uninstall the service
        /// </summary>
        protected virtual bool OnCommand_Uninstall() {
            return false;
        }
        #endregion
    }
}
