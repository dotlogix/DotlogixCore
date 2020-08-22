using DotLogix.Core.Utils.Naming;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.Conventions {
    public class DbNamingStrategy : DbNamingStrategyBase {
        public DbNamingStrategy(INamingStrategy strategy) {
            Strategy = strategy;
        }

        public static IDbNamingStrategy PascalCas => new DbNamingStrategy(NamingStrategies.PascalCase);
        public static IDbNamingStrategy CamelCase => new DbNamingStrategy(NamingStrategies.CamelCase);
        public static IDbNamingStrategy KebapCase => new DbNamingStrategy(NamingStrategies.KebapCase);
        public static IDbNamingStrategy LowerCase => new DbNamingStrategy(NamingStrategies.LowerCase);
        public static IDbNamingStrategy UpperCase => new DbNamingStrategy(NamingStrategies.UpperCase);
        public static IDbNamingStrategy SnakeCase => new DbNamingStrategy(NamingStrategies.SnakeCase);
        public static IDbNamingStrategy UpperSnakeCase => new DbNamingStrategy(NamingStrategies.UpperSnakeCase);

        public INamingStrategy Strategy { get; }

        public override string Rewrite(string value) {
            return Strategy.Rewrite(value);
        }
    }
}