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
using System.Reflection;
using System.ServiceProcess;
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.Core.WindowsServices {
    public class WindowsService {
        public ApplicationMode Mode { get; set; }
        public string ProgramExecutable { get; }
        public string LogDirectory { get; }

        public string ServiceName { get; }


        public WindowsService(string serviceName, string logDirectory) {
            ProgramExecutable = Assembly.GetEntryAssembly().Location;
            LogDirectory = logDirectory;
            Mode = Environment.UserInteractive ? ApplicationMode.UserInteractive : ApplicationMode.Service;
            ServiceName = serviceName;
        }

        protected virtual void OnConfiguration(string[] args, EventLog eventLog) {
            InitializeLoggers(eventLog);
        }

        protected virtual void OnStart(string[] args) { }

        protected virtual void OnStop() {
            Log.Shutdown();
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

                var running = true;
                do {
                    var line = Console.ReadLine();
                    if(line == null)
                        continue;

                    var split = line.Split(' ');
                    var commandCount = split.Length - 1;
                    var commandArgs = new string[commandCount];
                    if(commandCount > 0)
                        Array.Copy(split, 1, commandArgs, 0, commandCount);
                    running = OnCommand(split[0], commandArgs);
                } while(running);
            } catch(Exception e) {
                Log.Critical(e);
            }
            OnStop();
        }

        protected virtual bool OnCommand(string command, string[] args) {
            switch(command) {
                case "exit":
                    return false;
                case "clear":
                    Console.Clear();
                    return true;
                default:
                    Console.WriteLine("Possible commands:\n\texit\n\tclear\n\thelp");
                    return true;
            }
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
