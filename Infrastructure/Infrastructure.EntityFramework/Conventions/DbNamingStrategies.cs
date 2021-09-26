using DotLogix.Core.Utils.Naming;

namespace DotLogix.Infrastructure.EntityFramework.Conventions {
    public static class DbNamingStrategies
    {
        /// <inheritdoc cref="NamingStrategies.None"/>
        public static IDbNamingStrategy None => new DbNamingStrategy(NamingStrategies.None);
        
        /// <inheritdoc cref="NamingStrategies.PascalCase"/>
        public static IDbNamingStrategy PascalCase => new DbNamingStrategy(NamingStrategies.PascalCase);
        
        /// <inheritdoc cref="NamingStrategies.CamelCase"/>
        public static IDbNamingStrategy CamelCase => new DbNamingStrategy(NamingStrategies.CamelCase);
        
        /// <inheritdoc cref="NamingStrategies.KebapCase"/>
        public static IDbNamingStrategy KebapCase => new DbNamingStrategy(NamingStrategies.KebapCase);
        
        /// <inheritdoc cref="NamingStrategies.LowerCase"/>
        public static IDbNamingStrategy LowerCase => new DbNamingStrategy(NamingStrategies.LowerCase);
        
        /// <inheritdoc cref="NamingStrategies.UpperCase"/>
        public static IDbNamingStrategy UpperCase => new DbNamingStrategy(NamingStrategies.UpperCase);
        
        /// <inheritdoc cref="NamingStrategies.SnakeCase"/>
        public static IDbNamingStrategy SnakeCase => new DbNamingStrategy(NamingStrategies.SnakeCase);
        
        /// <inheritdoc cref="NamingStrategies.UpperSnakeCase"/>
        public static IDbNamingStrategy UpperSnakeCase => new DbNamingStrategy(NamingStrategies.UpperSnakeCase);
        
        public static IDbNamingStrategy Sanitized(INamingStrategy namingStrategy, string prefix = null, string suffix = "Entity") {
            return new SanitizingDbNamingStrategy(namingStrategy, prefix, suffix);
        }
    }
}