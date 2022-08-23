using System;

namespace DotLogix.WebServices.Testing.EntityFramework.Server; 

public class PostgresDatabase
{
    public PostgresDatabase(PostgresServer server)
    {
        Server = server;
    }

    public PostgresServer Server { get; set; }
    public Guid Guid { get; } = Guid.NewGuid();
    public string ConnectionString => $"server=127.0.0.1;port={Server.Port};database=enscape_test_{Guid:N};username=postgres;password=postgres";
}