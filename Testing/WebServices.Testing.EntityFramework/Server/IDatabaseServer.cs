using System;
using System.Threading.Tasks;

namespace DotLogix.WebServices.Testing.EntityFramework.Server; 

public interface IDatabaseServer : IDisposable
{
    string ConnectionString { get; }
    string LogDirectory { get; }

    Task StartAsync();
    Task StopAsync();
}