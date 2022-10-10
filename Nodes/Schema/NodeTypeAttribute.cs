using System;
using System.Runtime.CompilerServices;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Nodes {
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeTypeAttribute : Attribute {
        public Type ConverterFactory { get; set; }
        public Type NamingStrategy { get; set; }
        /// <summary>
        ///     The inner settings
        /// </summary>
        public ISettings Settings { get; set; } = new Settings(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings Default => new ConverterSettings();

        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        public string TimeFormat {
            get => GetWithMemberName("u");
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        public string NumberFormat {
            get => GetWithMemberName("G");
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        public string GuidFormat {
            get => GetWithMemberName("D");
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        public string EnumFormat {
            get => GetWithMemberName("D");
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        public EmitMode EmitMode {
            get => GetWithMemberName(EmitMode.Emit);
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
            Settings.Set(memberName, value);
        }

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected T GetWithMemberName<T>(T defaultValue = default, [CallerMemberName] string memberName = null) {
            return Settings.Get(memberName, defaultValue);
        }
    }
}