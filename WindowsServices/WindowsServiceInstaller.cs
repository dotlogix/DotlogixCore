// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WindowsServiceInstaller.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  21.02.2018
// ==================================================

#region
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
#endregion

namespace DotLogix.Core.WindowsServices {
    [RunInstaller(true)]
    public abstract class WindowsServiceInstaller : Installer {
        public string ServiceName { get; }
        public ServiceStartMode StartMode { get; }
        public ServiceAccount Account { get; }
        public bool RunAfterInstallation { get; }

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

        private void ServerInstaller_AfterInstall(object sender, InstallEventArgs e) {
            using(var sc = new ServiceController(ServiceName))
                sc.Start();
        }
    }
}
