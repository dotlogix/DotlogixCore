using System;
using System.Diagnostics;

namespace DotLogix.WebServices.Testing.Jobs; 

public sealed class NativeJob : IDisposable
{
    public IntPtr Handle { get; private set; }

    public NativeJob(IntPtr handle)
    {
        Handle = handle;
    }

    public NativeJob()
    {
        Handle = JobNativeMethods.CreateJobHandle();
    }

    public void AttachProcess(Process process)
    {
        if (Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(Handle));
        }

        JobNativeMethods.AttachProcessToJob(Handle, process);
    }

    public void AttachProcess(int processId)
    {
        var process = Process.GetProcessById(processId);
        AttachProcess(process);
    }

    public void Dispose()
    {
        if (Handle == IntPtr.Zero)
        {
            return;
        }

        JobNativeMethods.CloseJobHandle(Handle);
        Handle = IntPtr.Zero;
    }
}