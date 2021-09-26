#region
using DotLogix.Core.Utils.Naming;
#endregion

namespace DotLogix.Infrastructure.EntityFramework.Conventions {
    /// <summary>
    /// An implementation of the <see cref="IDbNamingStrategy"/> which removes pre- and suffixes of table names
    /// </summary>
    public class SanitizingDbNamingStrategy : DbNamingStrategy {
        private readonly string _suffix;
        private readonly string _prefix;
        
        /// <summary>
        /// Creates a new instance of <see cref="SanitizingDbNamingStrategy"/>
        /// </summary>
        public SanitizingDbNamingStrategy(INamingStrategy namingStrategy, string prefix = null, string suffix = "Entity") : base(namingStrategy)
        {
            _suffix = suffix;
            _prefix = prefix;
        }

        /// <inheritdoc />
        public override string RewriteTableName(string tableName) {
            tableName = Sanitize(tableName);
            return base.RewriteTableName(tableName);
        }

        /// <summary>
        /// Removes a pre- and suffix from the name
        /// </summary>
        protected virtual string Sanitize(string name) {
            var hasPrefix = _prefix != null && name.StartsWith(_prefix);
            var hasSuffix = _suffix != null && name.EndsWith(_suffix);

            if(hasPrefix && hasSuffix) {
                name = name[_prefix.Length..^_suffix.Length];
            } else if(hasPrefix) {
                name = name[_prefix.Length..];
            } else if(hasSuffix) {
                name = name[..^_suffix.Length];
            }

            return name;
        }
    }
}
