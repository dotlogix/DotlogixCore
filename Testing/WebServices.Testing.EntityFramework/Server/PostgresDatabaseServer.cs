using System;
using System.Threading.Tasks;
using DotLogix.Core.Diagnostics;

namespace DotLogix.WebServices.Testing.EntityFramework.Server; 

public class PostgresDatabaseServer : IDatabaseServer, IDisposable
{
    private readonly PostgresServer _server;
    private PostgresDatabase _database;

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public PostgresDatabaseServer(string postgresRootPath, ILogSource<PostgresDatabaseServer> logSource)
    {
        _server = new PostgresServer(postgresRootPath, logSource);
    }

    public string ConnectionString => _database?.ConnectionString;
    public string LogDirectory => _server.LogPath;

    public async Task StartAsync()
    {
        await _server.StartAsync();
        _database = new PostgresDatabase(_server);
    }

    public async Task StopAsync()
    {
        await _server.StopAsync();
        _database = null;
    }

    public void Dispose()
    {
        _server?.Dispose();
        _database = null;
    }
}