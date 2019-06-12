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
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using DotLogix.Core.Collections;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.WindowsServices {
    /// <summary>
    ///     A base for windows services
    /// </summary>
    public class WindowsService {
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
        ///     Creates a new instance of <see cref="WindowsService" />
        /// </summary>
        public WindowsService(string serviceName, string logDirectory) {
            ProgramExecutable = Assembly.GetEntryAssembly()?.Location;
            LogDirectory = logDirectory;
            Mode = Environment.UserInteractive ? ApplicationMode.UserInteractive : ApplicationMode.Service;
            ServiceName = serviceName;

            bool NoArgs(ConsoleCommand cmd, CommandArgs args) => args.IsEmpty;

            Commands.AddOrUpdate(new LambdaConsoleCommand("exit", "exit", (c, a) => OnCommand_Exit(), NoArgs));
            Commands.AddOrUpdate(new LambdaConsoleCommand("help", "help", (c, a) => OnCommand_Help(), NoArgs));
            Commands.AddOrUpdate(new LambdaConsoleCommand("clear", "clear", (c, a) => OnCommand_Clear(), NoArgs));
        }

        /// <summary>
        ///     A callback to start the service
        /// </summary>
        protected virtual void OnStart(string[] args) { }

        /// <summary>
        ///     A callback to stop the service
        /// </summary>
        protected virtual void OnStop() {
            Log.Shutdown();
        }

        /// <summary>
        ///     Start the service loop
        /// </summary>
        public void Run(string[] args) {
            switch(Mode) {
                case ApplicationMode.UserInteractive:
                    OnConfiguration(args, null);
                    UserInteractiveStartup(args);
                    break;
                case ApplicationMode.Service:
                    var serviceBase = new ServiceWrapper(this, ServiceName);
                    OnConfiguration(args, serviceBase.EventLog);
                    ServiceBase.Run(serviceBase);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Create the required loggers
        /// </summary>
        protected virtual void InitializeLoggers(EventLog eventLog) {
            var loggers = new List<ILogger>();
            if(LogDirectory != null)
                loggers.Add(new FileLogger(LogDirectory));

            if(Mode == ApplicationMode.UserInteractive)
                loggers.Add(new ConsoleLogger(150, 30));
            else if(eventLog != null)
                loggers.Add(new EventLogLogger(eventLog));

            if(loggers.Count > 0) {
                Log.LogLevel = LogLevels.Trace;
                Log.AttachLoggers(loggers);
                Log.Initialize();
            }
        }

        /// <summary>
        ///     A callback to configure the service
        /// </summary>
        protected virtual void OnConfiguration(string[] args, EventLog eventLog) {
            InitializeLoggers(eventLog);
        }

        private void UserInteractiveStartup(string[] args) {
            if(args.Length > 0) {
                switch(args[0].ToLower()) {
                    case "--install":
                    case "--i":
                        if(InstallService()) {
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
                        if(UninstallService()) {
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

                var commandResult = ConsoleCommandResult.Continue;
                var commandParserRegex = new Regex("\\s(?:(?<name>\\w+)[=:])\\s*(?:(?<value>(?:\\w+)|(?:\"([^\"]+?)\")))");
                do {
                    var line = Console.ReadLine();
                    if(line == null)
                        continue;

                    var commandName = line.SubstringUntil(' ');

                    var commandArgs = new Dictionary<string, string>();
                    var unnamedCommandArgs = new List<string>();
                    if(commandName.Length > line.Length) {
                        var argsMatches = commandParserRegex.Matches(line, commandName.Length);

                        foreach(Match match in argsMatches) {
                            if(!match.Success)
                                continue;

                            var name = match.Groups["name"].GetValueOrDefault();
                            var value = match.Groups["value"].GetValueOrDefault();

                            if(name == null)
                                unnamedCommandArgs.Add(value);
                            else
                                commandArgs[name] = value;
                        }
                    }

                    commandResult = OnCommand(commandName, new CommandArgs(commandArgs, unnamedCommandArgs));
                } while(commandResult.Exit == false);

                Environment.ExitCode = commandResult.ExitCode;
            } catch(Exception e) {
                Log.Critical(e);
            }

            OnStop();
        }

        private class ServiceWrapper : ServiceBase {
            private readonly WindowsService _app;

            public ServiceWrapper(WindowsService app, string serviceName) {
                _app = app;
                ServiceName = serviceName;
            }

            protected override void OnStart(string[] args) {
                _app.OnStart(args);
                base.OnStart(args);
            }

            protected override void OnStop() {
                _app.OnStop();
                base.OnStop();
            }
        }

        #region Install
        /// <summary>
        ///     A callback to install the service
        /// </summary>
        protected virtual bool InstallService() {
            try {
                ManagedInstallerClass.InstallHelper(new[] {"/i", ProgramExecutable});
                return true;
            } catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }


        /// <summary>
        ///     A callback to uninstall the service
        /// </summary>
        protected virtual bool UninstallService() {
            try {
                ManagedInstallerClass.InstallHelper(new[] {"/u", ProgramExecutable});
                return true;
            } catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }
        #endregion

        #region OnCommand
        /// <summary>
        ///     A callback to handle a user command
        /// </summary>
        protected virtual ConsoleCommandResult OnCommand(string command, CommandArgs args) {
            if(Commands.TryGet(command, out var cmd)) {
                if(cmd.OnValidate(args) == false) {
                    Console.WriteLine("Invalid usage of command " + cmd.Name);
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return ConsoleCommandResult.Continue;
                }

                if((args.Unnamed.Count == 1) && (args.Unnamed[0] == "help")) {
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return ConsoleCommandResult.Continue;
                }

                return cmd.OnExecute(args);
            }

            Console.WriteLine("Command could not be found possible commands are");
            OnCommand_Help();
            return ConsoleCommandResult.Continue;
        }

        private ConsoleCommandResult OnCommand_Exit() {
            return ConsoleCommandResult.ExitNoError;
        }

        private void OnCommand_Help() {
            foreach(var consoleCommand in Commands.OrderBy(c => c.Name))
                Console.WriteLine(consoleCommand.Description);
        }

        private void OnCommand_Clear() {
            Console.Clear();
        }
        #endregion
    }
}
