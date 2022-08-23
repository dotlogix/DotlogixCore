using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;
using DotLogix.WebServices.Testing.Tools;

namespace DotLogix.WebServices.Testing.EntityFramework.Server; 

public class PostgresServer : ServerInstance
{
    private const int MaxCleanupRetries = 10;
    private const int MaxPidRetrievalRetries = 10;
    private const int CleanupRetryDelay = 500;
    private const int PidRetrievalRetryDelay = 500;
    private const int ProcessExitTimeout = 2000;

    private Process _postgresProcess;
    public Guid InstanceGuid { get; } = Guid.NewGuid();
    public bool IsInitialized { get; private set; }

    public string PostgresRootPath { get; }
    public string BinPath => Path.Combine(PostgresRootPath, "bin");
    public string InstancePath => Path.Combine(PostgresRootPath, "instances", $"{InstanceGuid:N}");
    public string DataPath => Path.Combine(InstancePath, "data");
    public string LogPath => Path.Combine(InstancePath, "logs");

    public PostgresServer(string postgresRootPath, ILogSource logSource) : base(logSource) {
        PostgresRootPath = postgresRootPath;
    }

    protected override async Task StartInstanceAsync()
    {
        if (IsInitialized == false)
        {
            Directory.CreateDirectory(InstancePath);
            Directory.CreateDirectory(DataPath);
            Directory.CreateDirectory(LogPath);

            await RunScriptAsync("init_pg.cmd", $"{InstanceGuid:N}");
            IsInitialized = true;
        }

        await RunScriptAsync("start_pg.cmd", $"{InstanceGuid:N} {Port}");

        var pid = await GetPostgresPidAsync();
        if (pid.HasValue == false)
        {
            throw new ExternalException("Server start failed. File postmaster.pid does not exist");
        }

        _postgresProcess = Process.GetProcessById(pid.Value);
        Job.AttachProcess(_postgresProcess);
    }

    protected override async Task StopInstanceAsync()
    {
        await RunScriptAsync("stop_pg.cmd", $"{InstanceGuid:N}");

        _postgresProcess.WaitForExit(ProcessExitTimeout);
        _postgresProcess = null;
        await CleanupDataAsync();
    }

    private async Task CleanupDataAsync()
    {
        for (var i = 0; i < MaxCleanupRetries; i++)
        {
            try
            {
                Directory.Delete(InstancePath, true);
                return;
            }
            catch
            {
                // ignore file errors because of shared access with postgres process
                await Task.Delay(CleanupRetryDelay);
            }
        }
    }

    private async Task<int?> GetPostgresPidAsync()
    {
        // Wait for server start for a maximum of 5s
        for (var i = 0; i < MaxPidRetrievalRetries; i++)
        {
            var postmasterPath = Path.Combine(DataPath, "postmaster.pid");
            if (File.Exists(postmasterPath) == false)
            {
                await Task.Delay(PidRetrievalRetryDelay);
                continue;
            }

            try
            {
                var postmasterPid = await File.OpenText(postmasterPath).ReadLineAsync();
                return int.Parse(postmasterPid!);
            }
            catch
            {
                // ignore file errors because of shared access with postgres process
                await Task.Delay(PidRetrievalRetryDelay);
            }
        }

        return null;
    }

    private Task<bool> RunScriptAsync(string fileName, string arguments)
    {
        var path = Path.Combine(PostgresRootPath, fileName);
        return RunProcessAsync(PostgresRootPath, path, arguments);
    }
}