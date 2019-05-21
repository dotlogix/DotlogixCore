using System;
using System.Globalization;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    public class ConverterSettings {
        public static ConverterSettings Default => new ConverterSettings();
        public INamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();

        public string TimeFormat { get; set; } = "u";
        public string NumberFormat { get; set; } = "G";
        public string GuidFormat { get; set; } = "D";
        public string EnumFormat { get; set; } = "D";

        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
    }
}