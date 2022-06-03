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
        ///     A collection of commands for user interactive mode
        /// </summary>
        protected CommandProcessor CommandProcessor { get; }

        /// <summary>
        ///     The configuration of this service
        /// </summary>
        protected IConfiguration Configuration { get; set; }
        
        /// <summary>
        ///     The service provider of this service
        /// </summary>
        protected IServiceProvider ServiceProvider { get; set; }
        
        /// <summary>
        ///     The logger of this service
        /// </summary>
        protected ILogSource LogSource { get; set; }

        /// <summary>
        ///     Creates a new instance of <see cref="Service" />
        /// </summary>
        public Service(string serviceName) {
            ServiceName = serviceName;
            CommandProcessor = new CommandProcessor();
            CommandProcessor.Commands.Add(
                new LambdaConsoleCommand(
                    "exit",
                    "Closes the application",
                    (_, _) => Task.CompletedTask
                )
            );
        }

        /// <summary>
        ///     A callback to start the service
        /// </summary>
        protected virtual Task OnStartAsync(string[] args) {
            var builder = new ConfigurationBuilder();
            OnConfigure(builder, args);
            Configuration = builder.Build();

            var services = new ServiceCollection();
            OnConfigureServices(services, args);
            ServiceProvider = services.BuildServiceProvider();
                
            var logSourceProvider = ServiceProvider.GetRequiredService<ILogSourceProvider>();
            LogSource = logSourceProvider.Create(GetType());
            return Task.CompletedTask;
        }

        /// <summary>
        ///     A callback to stop the service
        /// </summary>
        protected virtual Task OnStopAsync() {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Start the service loop
        /// </summary>
        public virtual async Task RunAsync(string[] args) {
            try {
                await OnStartAsync(args).ConfigureAwait(false);
                await OnProcessUserInputAsync().ConfigureAwait(false);
            } catch (Exception e) {
                LogSource.Critical(e);
                Environment.ExitCode = e.HResult; // error code
            } finally {
                await OnStopAsync().ConfigureAwait(false);
            }
        }

        #region Configuring

        protected virtual void OnConfigureServices(IServiceCollection services, string[] args) {
            
        }

        protected virtual void OnConfigure(IConfigurationBuilder configuration, string[] args) {
            
        }
        #endregion


        #region OnCommand
        protected virtual async Task OnProcessUserInputAsync() {
            var input = Console.In;
            string line;
            while((line = await input.ReadLineAsync()) is not "exit") {
                if(line is null)
                    continue;
                await CommandProcessor.ProcessAsync(line);
            }
        }
        #endregion
    }
}
