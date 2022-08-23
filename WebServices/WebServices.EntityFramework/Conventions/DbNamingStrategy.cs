#region
using DotLogix.Core.Utils.Naming;
#endregion

namespace DotLogix.WebServices.EntityFramework.Conventions; 

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
        
    /// <inheritdoc cref="Equals(object)"/>
    protected bool Equals(DbNamingStrategy other) {
        return Equals(Strategy, other.Strategy);
    }
        
    /// <inheritdoc/>
    public override bool Equals(object obj) {
        if(ReferenceEquals(null, obj)) {
            return false;
        }

        if(ReferenceEquals(this, obj)) {
            return true;
        }

        if(obj.GetType() != this.GetType()) {
            return false;
        }

        return Equals((DbNamingStrategy)obj);
    }

    /// <inheritdoc/>
    public override int GetHashCode() {
        return (Strategy is not null ? Strategy.GetHashCode() : 0);
    }
}