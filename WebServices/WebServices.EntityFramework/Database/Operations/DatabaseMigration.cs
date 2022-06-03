using System;
using System.Globalization;

namespace DotLogix.WebServices.EntityFramework.Database {

    public class DatabaseMigration
    {
        public string Name { get; }
        public string FullName { get; }
        public DateTime Timestamp { get; }

        public DatabaseMigration(string migrationName) {
            FullName = migrationName;

            var partialNames = migrationName.Split('_', 2);
            if (partialNames.Length != 2)
            {
                Name = migrationName;
                return;
            }

            Name = partialNames[1];
            const DateTimeStyles universalStyle = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
            if (DateTime.TryParseExact(partialNames[0], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, universalStyle, out var dt))
            {
                Timestamp = dt;
            }
        }
    }
}