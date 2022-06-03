using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Testing.Jobs;

namespace DotLogix.WebServices.Testing.Tools
{
    public abstract class ServerInstance : IDisposable
    {
        private static int _globalPortCounter = 30750;
        private const int RetryDelay = 500;

        protected ServerInstance(ILogSource logSource)
        {
            LogSource = logSource;
            Job = new NativeJob();
        }

        protected NativeJob Job { get; }
        protected ILogSource LogSource { get; }

        public bool IsRunning { get; protected set; }
        public bool IsDisposed { get; protected set; }
        public int Port { get; protected set; }
        public int MaxRetries { get; set; } = 5;

        public async Task StartAsync()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (IsRunning)
            {
                return;
            }

            // Retry startup a couple of times because of possible Port issues
            for (int i = 1; i <= MaxRetries; i++)
            {
                try
                {
                    Port = GetNextUnusedPort();
                    await StartInstanceAsync();
                    IsRunning = true;
                    return;
                }
                catch
                {
                    await Task.Delay(RetryDelay);
                    if (i == MaxRetries)
                    {
                        throw;
                    }
                }
            }
        }

        public async Task StopAsync()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (IsRunning == false)
            {
                return;
            }

            await StopInstanceAsync();
            IsRunning = false;
        }

        protected abstract Task StartInstanceAsync();
        protected abstract Task StopInstanceAsync();


        protected async Task<bool> RunProcessAsync(string workingDir, string fileName, string arguments, bool throwOnError = true)
        {
            using var worker = new ExternalProcessWorker(workingDir, fileName, arguments);
            worker.AttachToJob(Job);

            if (await worker.RunAsync())
            {
                LogSource.Info(worker.StandardOutput);
                return true;
            }

            LogSource.Error(worker.ErrorOutput);
            if (throwOnError)
            {
                throw new ExternalProcessException(worker);
            }

            return false;
        }

        protected int GetNextUnusedPort()
        {
            var ips = IPGlobalProperties.GetIPGlobalProperties();
            var connections = ips.GetActiveTcpConnections();
            var listeners = ips.GetActiveTcpListeners();
            var usedPorts = new HashSet<int>(connections.Length + listeners.Length);
            usedPorts.UnionWith(connections.Select(p => p.LocalEndPoint.Port));
            usedPorts.UnionWith(listeners.Select(p => p.Port));

            int currentPort;
            do
            {
                currentPort = Interlocked.Increment(ref _globalPortCounter);
            } while (usedPorts.Contains(currentPort));

            return currentPort;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == false || IsDisposed)
            {
                return;
            }

            StopAsync().GetAwaiter().GetResult();
            Job?.Dispose();
            IsDisposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}