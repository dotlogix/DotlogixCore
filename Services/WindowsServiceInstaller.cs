// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WindowsServiceInstaller.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
#endregion

namespace DotLogix.Core.WindowsServices {
    /// <summary>
    /// The internal service installer
    /// </summary>
    [RunInstaller(true)]
    public abstract class WindowsServiceInstaller : Installer {
        /// <summary>
        /// The service name
        /// </summary>
        public string ServiceName { get; }
        /// <summary>
        /// The service start mode
        /// </summary>
        public ServiceStartMode StartMode { get; }
        /// <summary>
        /// The service account
        /// </summary>
        public ServiceAccount Account { get; }
        /// <summary>
        /// A flag to determine if the service should start after installation
        /// </summary>
        public bool RunAfterInstallation { get; }

        /// <summary>
        /// Creates a new instance of <see cref="WindowsServiceInstaller"/>
        /// </summary>
        protected WindowsServiceInstaller(string serviceName, ServiceStartMode startMode = ServiceStartMode.Automatic, ServiceAccount account = ServiceAccount.LocalSystem, bool runAfterInstallation = true) {
            ServiceName = serviceName;
            StartMode = startMode;
            Account = account;
            RunAfterInstallation = runAfterInstallation;

            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            processInstaller.Account = account;

            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.StartType = startMode;

            serviceInstaller.ServiceName = serviceName;
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

            if(runAfterInstallation)
                AfterInstall += ServerInstaller_AfterInstall;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ServerInstaller_AfterInstall(object sender, InstallEventArgs e) {
            if(!RunAfterInstallation)
                return;

            using(var sc = new ServiceController(ServiceName))
                sc.Start();
        }
    }
}
