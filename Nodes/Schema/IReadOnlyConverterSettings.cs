using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes.Schema {
    public interface IReadOnlyConverterSettings {
        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        string TimeFormat { get; }

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        string NumberFormat { get; }

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        string GuidFormat { get; }

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        string EnumFormat { get; }

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        EmitMode EmitMode { get; }

        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        INamingStrategy NamingStrategy { get; }

        bool ShouldEmitValue(object value);

        /// <inheritdoc />
        IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null);
    }
}