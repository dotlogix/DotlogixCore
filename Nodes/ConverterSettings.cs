using System;
using System.Globalization;
using DotLogix.Core.Extensions;
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

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public ConverterSettings() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public ConverterSettings(ConverterSettings template) {
            TimeFormat = template.TimeFormat;
            NumberFormat = template.NumberFormat;
            GuidFormat = template.GuidFormat;
            EnumFormat = template.EnumFormat;
            EmitDefault = template.EmitDefault;
            EmitNull = template.EmitNull;
            FormatProvider = template.FormatProvider;
            EmitDefinedTypeOnly = template.EmitDefinedTypeOnly;
        }

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
        /// Determines if default values should be ignored
        /// </summary>
        public bool EmitDefault { get; set; } = true;

        /// <summary>
        /// Determines if null values should be ignored
        /// </summary>
        public bool EmitNull { get; set; } = true;

        /// <summary>
        /// Determines if only the defined type should be instead of the actual instance type
        /// </summary>
        public bool EmitDefinedTypeOnly { get; set; } = false;

        /// <summary>
        /// The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

        public bool ShouldEmitValue(object value)
        {
            if (EmitDefault == false)
                return Equals(value?.GetType().GetDefaultValue(), value) == false;
            if (EmitNull == false)
                return Equals(value, null) == false;
            return true;
        }
    }
}