namespace DotLogix.Core.Utils.Naming {
    /// <summary>
    ///     An interface to represent naming strategies
    /// </summary>
    public interface INamingStrategy {
        /// <summary>
        ///     Rewrites the name according to the naming strategy
        /// </summary>
        string Rewrite(string name);
    }
}