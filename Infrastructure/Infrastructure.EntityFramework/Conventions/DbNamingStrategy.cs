#region
using DotLogix.Core.Utils.Naming;
#endregion

namespace DotLogix.Infrastructure.EntityFramework.Conventions {
    /// <summary>
    /// An implementation of the <see cref="IDbNamingStrategy"/> based on a <see cref="INamingStrategy"/>
    /// </summary>
    public class DbNamingStrategy : DbNamingStrategyBase {
        /// <summary>
        /// The naming strategy to use
        /// </summary>
        protected INamingStrategy Strategy { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="DbNamingStrategy" />
        /// </summary>
        public DbNamingStrategy(INamingStrategy strategy) {
            Strategy = strategy;
        }

        /// <inheritdoc />
        public override string Rewrite(string value) {
            return Strategy.Rewrite(value);
        }
    }
}
