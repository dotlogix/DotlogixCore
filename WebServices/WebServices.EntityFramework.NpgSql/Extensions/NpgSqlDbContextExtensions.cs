using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;

namespace DotLogix.WebServices.EntityFramework.Extensions
{
    public static class NpgSqlDbContextExtensions
    {
        public static async Task NpgSqlReloadTypesAsync(this DatabaseFacade database)
        {
            // Required for pg-extensions to work properly
            await database.OpenConnectionAsync();

            if (database.GetDbConnection() is NpgsqlConnection npgsqlConnection)
            {
                npgsqlConnection.ReloadTypes();
            }
        }
    }
}