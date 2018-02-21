// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WindowsService.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  21.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using DotLogix.Core.Diagnostics;
#endregion

namespace DotLogix.Core.WindowsServices {
    public class WindowsService {
        public ApplicationMode Mode { get; set; }
        public string ProgramDirectory { get; }
        public string ProgramExecutable { get; }
        public string LogDirectory { get; }

        public string ServiceName { get; }

        public WindowsService(string serviceName, string logDirectory) {
            ProgramExecutable = Assembly.GetEntryAssembly().Location;
            ProgramDirectory = Path.GetDirectoryName(ProgramExecutable);
            LogDirectory = logDirectory != null ? GetFullPath(logDirectory) : null;
            Mode = Environment.UserInteractive ? ApplicationMode.UserInteractive : ApplicationMode.Service;
            ServiceName = serviceName;
        }

        protected virtual void OnStart(string[] args, EventLog eventLog) {
            InitializeLoggers(eventLog);
        }

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
                    UserInteractiveStartup(args);
                    break;
                case ApplicationMode.Service:
                    var serviceBase = new ServiceWrapper(this, ServiceName);
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
                OnStart(args, null);
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
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

        public string GetFullPath(params string[] path) {
            var pathParts = new string[path.Length - 1];
            Array.Copy(path, pathParts, 1);
            pathParts[0] = ProgramDirectory;
            return Path.Combine(pathParts);
        }

        public string GetFullPath(string path1) {
            return Path.Combine(ProgramDirectory, path1);
        }

        public string GetFullPath(string path1, string path2) {
            return Path.Combine(ProgramDirectory, path1, path2);
        }

        public string GetFullPath(string path1, string path2, string path3) {
            return Path.Combine(ProgramDirectory, path1, path2, path3);
        }

        private class ServiceWrapper : ServiceBase {
            private readonly WindowsService _app;

            public ServiceWrapper(WindowsService app, string serviceName) {
                _app = app;
                ServiceName = serviceName;
            }

            protected override void OnStart(string[] args) {
                _app.OnStart(args, EventLog);
                base.OnStart(args);
            }

            protected override void OnStop() {
                _app.OnStop();
                base.OnStop();
            }
        }
    }
}
