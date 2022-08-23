// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WindowsService.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  21.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Services.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace DotLogix.Core.Services {
    /// <summary>
    ///     A base class for service like applications
    /// </summary>
    public class Service {
        /// <summary>
        ///     The service name
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        ///     The command processor of this service
        /// </summary>
        protected ICommandProcessor CommandProcessor { get; private set; }

        /// <summary>
        ///     The configuration of this service
        /// </summary>
        protected IConfiguration Configuration { get; private set; }

        /// <summary>
        ///     The service provider of this service
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///     The default log source of this service
        /// </summary>
        protected ILogSource LogSource { get; set; }

        /// <summary>
        ///     Creates a new instance of <see cref="Service" />
        /// </summary>
        public Service(string serviceName) {
            ServiceName = serviceName;
        }

        /// <summary>
        ///     A callback to start the service
        /// </summary>
        protected virtual Task OnStartAsync(string[] args) {
            Configuration = CreateConfiguration(args);
            ServiceProvider = CreateServiceProvider(args);
            CommandProcessor = CreateCommandProcessor(args);
            LogSource = CreateLogSource();
            return Task.CompletedTask;
        }

        /// <summary>
        ///     A callback to stop the service
        /// </summary>
        protected virtual Task OnStopAsync() {
            Configuration = null;
            ServiceProvider = null;
            CommandProcessor = null;
            LogSource = null;
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Start the service loop
        /// </summary>
        public virtual async Task RunAsync(string[] args) {
            int? exitCode = null;
            try {
                await OnStartAsync(args).ConfigureAwait(false);
                exitCode = await OnProcessUserInputAsync().ConfigureAwait(false);
            } catch(Exception e) {
                LogSource.Critical(e);
                exitCode = e.HResult; // error code
            } finally {
                await OnStopAsync().ConfigureAwait(false);
                Environment.ExitCode = exitCode.GetValueOrDefault();
            }
        }

        protected virtual async Task<int?> OnProcessUserInputAsync() {
            var input = Console.In;
            int? exitCode = null;
            while(exitCode.HasValue == false) {
                var line = await input.ReadLineAsync();
                if(string.IsNullOrWhiteSpace(line)) continue;
                exitCode = await CommandProcessor.ProcessAsync(line);
            }

            return exitCode;
        }

        #region Logging
        protected virtual ILogSource CreateLogSource() {
            return ServiceProvider.GetRequiredService<ILogSourceProvider>().Create(GetType());
        }
        #endregion

        #region Configuration
        protected virtual IConfiguration CreateConfiguration(string[] args) {
            var configuration = new ConfigurationBuilder();
            OnConfigure(configuration, args);
            return configuration.Build();
        }

        protected virtual void OnConfigure(IConfigurationBuilder configuration, string[] args) {
        }
        #endregion

        #region Services
        protected virtual IServiceProvider CreateServiceProvider(string[] args) {
            var services = new ServiceCollection();
            OnConfigureServices(services, args);
            return services.BuildServiceProvider();
        }

        protected virtual void OnConfigureServices(IServiceCollection services, string[] args) {
        }
        #endregion

        #region Commands
        protected virtual ICommandProcessor CreateCommandProcessor(string[] args) {
            var processor = new CommandProcessorBuilder();
            processor.UseServiceProvider(ServiceProvider);
            OnConfigureCommands(processor, args);
            return processor.Build();
        }

        protected virtual void OnConfigureCommands(ICommandProcessorBuilder processor, string[] args) {
            processor.UseDefaultCommands();
        }
        #endregion
    }
}
