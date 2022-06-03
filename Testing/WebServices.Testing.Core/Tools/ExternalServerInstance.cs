using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;

namespace DotLogix.WebServices.Testing.Tools
{
    public abstract class ExternalServerInstance : ServerInstance
    {
        protected ExternalServerInstance(ILogSource logSource) : base(logSource)
        {
        }

        protected ExternalProcessWorker Worker { get; private set; }

        protected override Task StartInstanceAsync()
        {
            Worker = CreateWorker();
            Worker.AttachToJob(Job);
            Worker.Start();
            return Task.CompletedTask;
        }

        protected override Task StopInstanceAsync()
        {
            Worker.Stop();
            return Task.CompletedTask;
        }

        protected abstract ExternalProcessWorker CreateWorker();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing == false) return;

            Worker?.Dispose();
        }
    }
}