using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.Testing.Jobs;

namespace DotLogix.WebServices.Testing.Tools; 

public class ExternalProcessWorker : IDisposable
{
    private const int ProcessExitTimeout = 2000;
    public string WorkingDir { get; set; }
    public string FileName { get; set; }
    public string Arguments { get; set; }
    public bool CombineOutputLogs { get; set; }

    private int _processCount;
    private StringBuilder _errorLog;
    private StringBuilder _stdLog;

    private NativeJob _job;
    private Process _process;
    private TaskCompletionSource<object> _processExitCompletionSource;
    private CancellationTokenRegistration _processCancellationRegistration;

    public string ErrorOutput => _errorLog?.ToString();
    public string StandardOutput => _stdLog?.ToString();

    public int ExitCode { get; private set; }
    public bool IsRunning => _processCount > 0;
    public bool HasErrors { get; private set; }

    public ExternalProcessWorker()
    {
        CombineOutputLogs = true;
    }

    public ExternalProcessWorker(string workingDir, string fileName, string arguments, bool combineOutputLogs = true)
    {
        WorkingDir = workingDir;
        FileName = fileName;
        Arguments = arguments;
        CombineOutputLogs = combineOutputLogs;
    }

    public void AttachToJob(NativeJob job)
    {
        if (_processCount > 0)
        {
            throw new NotSupportedException("Can not change job while a process is running");
        }
        _job = job;
    }

    public bool Run(CancellationToken cancellationToken = default)
    {
        return RunAsync(cancellationToken).GetAwaiter().GetResult();
    }

    public async Task<bool> RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Start(cancellationToken);
            await _processExitCompletionSource.Task.ConfigureAwait(false);
            Stop();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Start(CancellationToken cancellationToken = default)
    {
        if (Interlocked.CompareExchange(ref _processCount, 1, 0) != 0)
        {
            throw new InvalidOperationException("Worker is already running. Can not start additional instances at the same time");
        }

        _process = CreateProcess();

        // reset
        ExitCode = 0;
        HasErrors = false;
        _stdLog = new StringBuilder();
        _errorLog = CombineOutputLogs ? _stdLog : new StringBuilder();

        // attach to cancellation token
        _processExitCompletionSource?.TrySetCanceled();
        _processCancellationRegistration.Dispose();
        _processExitCompletionSource = new TaskCompletionSource<object>();
        _processCancellationRegistration = cancellationToken.Register(() =>
        {
            _processExitCompletionSource.TrySetCanceled();
            Stop();
        });

        // register process events
        _process.Exited += Process_OnExited;
        _process.ErrorDataReceived += Process_OnErrorReceived;
        _process.OutputDataReceived += Process_OnOutputDataReceived;

        // start process
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _job?.AttachProcess(_process);
    }

    public void Stop()
    {
        OnProcessExit();
    }

    public void Dispose()
    {
        Stop();
        ExitCode = 0;
        HasErrors = false;
        _errorLog = null;
        _stdLog = null;

        _processExitCompletionSource?.TrySetCanceled();
        _processExitCompletionSource = new TaskCompletionSource<object>();
    }

    private Process CreateProcess()
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = WorkingDir,
                FileName = FileName,
                Arguments = Arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            },
            EnableRaisingEvents = true
        };
    }

    private void OnProcessExit()
    {
        if (Interlocked.CompareExchange(ref _processCount, 0, 1) != 1)
        {
            return;
        }

        if (_process.WaitForExit(ProcessExitTimeout) == false)
        {
            _process.Kill();
        }

        // exit process
        ExitCode = _process.ExitCode;
        _process.CancelOutputRead();
        _process.CancelErrorRead();

        // unregister process events
        _process.Exited -= Process_OnExited;
        _process.ErrorDataReceived -= Process_OnErrorReceived;
        _process.OutputDataReceived -= Process_OnOutputDataReceived;

        // reset
        _process?.Close();
        _process?.Dispose();
        _process = null;

        // signal process exit
        _processCancellationRegistration.Dispose();
        _processExitCompletionSource.TrySetResult(default);
    }

    private void Process_OnExited(object sender, EventArgs e)
    {
        OnProcessExit();
    }

    private void Process_OnErrorReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null)
        {
            OnProcessExit();
        }
        else
        {
            HasErrors = true;
            _errorLog?.AppendLine(e.Data);
        }
    }

    private void Process_OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null)
        {
            OnProcessExit();
        }
        else
        {
            _stdLog?.AppendLine(e.Data);
        }
    }
}