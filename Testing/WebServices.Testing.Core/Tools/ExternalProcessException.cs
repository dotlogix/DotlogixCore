using System;

namespace DotLogix.WebServices.Testing.Tools; 

public class ExternalProcessException : Exception
{
    public ExternalProcessException(ExternalProcessWorker worker)
    {
        ExitCode = worker.ExitCode;
        Output = worker.StandardOutput;
        Errors = worker.ErrorOutput;

        WorkingDir = worker.WorkingDir;
        FileName = worker.FileName;
        Arguments = worker.Arguments;
    }

    public int ExitCode { get; }
    public string Output { get; }
    public string Errors { get; }

    public string WorkingDir { get; set; }
    public string FileName { get; set; }
    public string Arguments { get; set; }

    public override string Message
    {
        get
        {
            var log = string.IsNullOrEmpty(Errors) ? Errors : Output;
            return $"Error while executing external tool \"{FileName} {Arguments}\" in directory \"{WorkingDir}\"\nError Details:\n" + log;
        }
    }
}