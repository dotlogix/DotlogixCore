using System.Threading.Tasks;

namespace DotLogix.WebServices.Testing.EntityFramework.Server
{
    public class ExternalDatabaseServerAdapter : IDatabaseServer
    {
        public string ConnectionString { get; }
        public string LogDirectory => null;

        public ExternalDatabaseServerAdapter(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Task StartAsync() => Task.CompletedTask;
        public Task StopAsync() => Task.CompletedTask;
        public void Dispose() { }
    }
}