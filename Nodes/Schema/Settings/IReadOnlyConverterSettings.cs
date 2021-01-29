using System;
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
        
        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        IFormatProvider FormatProvider { get; }
        
        /// <summary>
        ///     The converter resolver
        /// </summary>
        INodeConverterResolver Resolver { get; }

        /// <inheritdoc />
        IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null);

        bool ShouldEmitValue(object value);
    }
}