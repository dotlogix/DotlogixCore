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
    public class WindowsService {
        public ApplicationMode Mode { get; set; }
        public string ProgramExecutable { get; }
        public string LogDirectory { get; }

        public string ServiceName { get; }
        public KeyedCollection<string, ConsoleCommand> Commands { get; } = new KeyedCollection<string, ConsoleCommand>(c=>c.Name);


        public WindowsService(string serviceName, string logDirectory) {
            ProgramExecutable = Assembly.GetEntryAssembly().Location;
            LogDirectory = logDirectory;
            Mode = Environment.UserInteractive ? ApplicationMode.UserInteractive : ApplicationMode.Service;
            ServiceName = serviceName;

            bool NoArgs(ConsoleCommand cmd, string[] args) => args.Length == 0;

            Commands.AddOrUpdate(new ConsoleCommand("exit", "exit", (cmd, args) => 0, NoArgs));
            Commands.AddOrUpdate(new ConsoleCommand("help", "help", OnCommand_Help, NoArgs));
            Commands.AddOrUpdate(new ConsoleCommand("clear", "clear", OnCommand_Clear, NoArgs));
        }

        private int? OnCommand_Help(ConsoleCommand cmd, string[] args) {
            foreach(var consoleCommand in Commands.OrderBy(c=>c.Name))
                Console.WriteLine(consoleCommand.Description);
            return null;
        }
        private int? OnCommand_Clear(ConsoleCommand cmd, string[] args) {
            Console.Clear();
            return null;
        }

        protected virtual void OnConfiguration(string[] args, EventLog eventLog) {
            InitializeLoggers(eventLog);
        }

        protected virtual void OnStart(string[] args) { }

        protected virtual void OnStop() {
            Log.Shutdown();
        }

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

        protected virtual int? OnCommand(string command, string[] args) {
            if(Commands.TryGet(command, out var cmd)) {
                if(cmd.ValidateArguments != null && cmd.ValidateArguments.Invoke(cmd, args) == false) {
                    Console.WriteLine("Invalid usage of command "+cmd.Name);
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return null;
                }

                if(args.Length == 1 && args[0] == "help") {
                    Console.WriteLine(cmd.HelpText ?? cmd.Description);
                    return null;
                }

                return cmd.Callback.Invoke(cmd, args);
            }

            Console.WriteLine("Command could not be found possible commands are");
            OnCommand_Help(cmd, args);
            return null;
        }

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

                int? exitCode = null;
                var commandParserRegex = new Regex("(?:\\s(\\w+)|(?:\"([^\"]+)\"))+");
                do {
                    var line = Console.ReadLine();
                    if(line == null)
                        continue;

                    var commandName = line.SubstringUntil(' ');

                    string[] commandArgs;
                    if(commandName.Length == line.Length)
                        commandArgs = new string[0];
                    else {
                        var argsMatch = commandParserRegex.Match(line, commandName.Length);
                        if(argsMatch.Success) {
                            commandArgs = argsMatch.Groups.Cast<Group>().Skip(1).Where(g => g.Success).Select(c => c.Value.Trim()).ToArray();
                        } else
                            commandArgs = new[] {"help"};
                    }

                    exitCode = OnCommand(commandName, commandArgs);
                } while(exitCode.HasValue == false);

                Environment.ExitCode = exitCode.Value;
            } catch(Exception e) {
                Log.Critical(e);
            }
            OnStop();
        }

        private bool InstallService() {
            try {
                ManagedInstallerClass.InstallHelper(new[] {"/i", ProgramExecutable});
                return true;
            } catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }


        protected virtual bool UninstallService() {
            try {
                ManagedInstallerClass.InstallHelper(new[] {"/u", ProgramExecutable});
                return true;
            } catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
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
    }
}
