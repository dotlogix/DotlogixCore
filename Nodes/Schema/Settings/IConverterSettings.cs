using System;
using System.Globalization;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes.Schema {
    public interface IConverterSettings : IReadOnlyConverterSettings {
        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        new INamingStrategy NamingStrategy { get; set; }

        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        new string TimeFormat { get; set; }

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        new string NumberFormat { get; set; }

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        new string GuidFormat { get; set; }

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        new string EnumFormat { get; set; }
        
        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        new IFormatProvider FormatProvider { get; set; }

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        new EmitMode EmitMode { get; set; }
    }
}