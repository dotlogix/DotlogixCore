using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace DotLogix.WebServices.Testing.Jobs
{
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "StructCanBeMadeReadOnly")]
    public static class JobNativeMethods
    {
        public static IntPtr CreateJobHandle()
        {
            var job = CreateJobObject(null, null);
            if (job == IntPtr.Zero)
            {
                var error = Marshal.GetLastWin32Error();
                var message = new Win32Exception(error).Message;
                throw new Win32Exception(error, $"{nameof(CreateJobObject)} failed with error {error}: {message}");
            }

            var extendedLimitInformation = new JobObjectExtendedLimitInformation
            {
                BasicLimitInformation =
                {
                    LimitFlags = JobObjectLimitFlags.KillOnJobClose
                }
            };
            var length = Marshal.SizeOf<JobObjectExtendedLimitInformation>();
            var memory = Marshal.AllocHGlobal(length);
            try
            {
                Marshal.StructureToPtr(extendedLimitInformation, memory, false);
                if (SetInformationJobObject(job, JobObjectInfoType.ExtendedLimitInformation, memory, (uint) length))
                {
                    return job;
                }

                var error = Marshal.GetLastWin32Error();
                var message = new Win32Exception(error).Message;
                throw new Win32Exception(error, $"{nameof(SetInformationJobObject)} failed with error {error}: {message}");
            }
            finally
            {
                Marshal.FreeHGlobal(memory);
            }
        }

        public static void CloseJobHandle(IntPtr jobHandle)
        {
            if (CloseHandle(jobHandle))
            {
                return;
            }

            var error = Marshal.GetLastWin32Error();
            var message = new Win32Exception(error).Message;
            throw new Win32Exception(error, $"{nameof(SetInformationJobObject)} failed with error {error}: {message}");
        }

        public static void AttachProcessToJob(IntPtr jobHandle, Process process)
        {
            if (AssignProcessToJobObject(jobHandle, process.Handle))
            {
                return;
            }

            var error = Marshal.GetLastWin32Error();
            var message = new Win32Exception(error).Message;
            throw new Win32Exception(error, $"{nameof(AssignProcessToJobObject)} failed with error {error}: {message}");
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateJobObject(object jobAttributes, string jobName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CloseHandle(IntPtr jobHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetInformationJobObject(IntPtr jobHandle, JobObjectInfoType infoType, IntPtr jobObjectInfo, uint jobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AssignProcessToJobObject(IntPtr jobHandle, IntPtr processHandle);

        [StructLayout(LayoutKind.Sequential)]
        private struct JobObjectExtendedLimitInformation
        {
            public JobObjectBasicLimitInformation BasicLimitInformation;
            public IoCounters IoInfo;
            public UIntPtr ProcessMemoryLimit;
            public UIntPtr JobMemoryLimit;
            public UIntPtr PeakProcessMemoryUsed;
            public UIntPtr PeakJobMemoryUsed;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IoCounters
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct JobObjectBasicLimitInformation
        {
            public long PerProcessUserTimeLimit;
            public long PerJobUserTimeLimit;
            public JobObjectLimitFlags LimitFlags;
            public UIntPtr MinimumWorkingSetSize;
            public UIntPtr MaximumWorkingSetSize;
            public uint ActiveProcessLimit;
            public UIntPtr Affinity;
            public uint PriorityClass;
            public uint SchedulingClass;
        }

        private enum JobObjectInfoType
        {
            BasicLimitInformation = 2,
            BasicUiRestrictions = 4,
            EndOfJobTimeInformation = 6,
            ExtendedLimitInformation = 9,
            SecurityLimitInformation = 5,
            AssociateCompletionPortInformation = 7,
            GroupInformation = 11
        }

        [Flags]
        private enum JobObjectLimitFlags : uint
        {
            JobObjectLimitWorkingSet = 1,
            JobObjectLimitProcessTime = 2,
            JobObjectLimitJobTime = 4,
            JobObjectLimitActiveProcess = 8,
            JobObjectLimitAffinity = 16,
            JobObjectLimitPriorityClass = 32,
            JobObjectLimitPreserveJobTime = 64,
            JobObjectLimitSchedulingClass = 128,
            JobObjectLimitProcessMemory = 256,
            JobObjectLimitJobMemory = 512,
            JobObjectLimitDieOnUnhandledException = 1024,
            JobObjectLimitBreakawayOk = 2048,
            JobObjectLimitSilentBreakawayOk = 4096,
            KillOnJobClose = 8192,
            JobObjectLimitSubsetAffinity = 16384
        }
    }
}