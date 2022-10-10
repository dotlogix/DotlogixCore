using System;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Nodes {
    public interface IReadOnlyConverterSettings {
        /// <summary>
        ///     The inner settings
        /// </summary>
        IReadOnlySettings Settings { get; }
        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        INamingStrategy NamingStrategy { get; }

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
        ///     The child settings for collection types
        /// </summary>
        IReadOnlyConverterSettings ChildSettings { get;}

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        IFormatProvider FormatProvider { get; set; }
        IConverterSettings GetScoped(TypeSettings typeSettings = null, MemberSettings memberSettings = null);
        bool ShouldEmitValue(object value);
    }

    public interface IConverterSettings : IReadOnlyConverterSettings {
        /// <summary>
        ///     The inner settings
        /// </summary>
        new ISettings Settings { get; }

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
        ///     Determines if default or null values should be ignored
        /// </summary>
        new EmitMode EmitMode { get; set; }
        /// <summary>
        ///     The child settings for collection types
        /// </summary>
        new IConverterSettings ChildSettings { get; set; }
    }
}