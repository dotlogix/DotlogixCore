using System;
using System.Globalization;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// Basic node converter settings
    /// </summary>
    public class ConverterSettings {
        /// <summary>
        /// The default settings
        /// </summary>
        public static ConverterSettings Default => new ConverterSettings();
        /// <summary>
        /// The naming strategy (camelCase by default)
        /// </summary>
        public INamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();

        /// <summary>
        /// The time format (u by default)
        /// </summary>
        public string TimeFormat { get; set; } = "u";
        /// <summary>
        /// The number format (G by default)
        /// </summary>
        public string NumberFormat { get; set; } = "G";
        /// <summary>
        /// The guid format (D by default)
        /// </summary>
        public string GuidFormat { get; set; } = "D";
        /// <summary>
        /// The enum format (D by default)
        /// </summary>
        public string EnumFormat { get; set; } = "D";

        /// <summary>
        /// The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
    }
}