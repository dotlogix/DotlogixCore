#region using
using System;
using System.Globalization;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
#endregion

namespace DotLogix.Core.Nodes {
    public class ScopedConverterSettings : IReadOnlyConverterSettings {
        /// <summary>
        ///     The scope type settings
        /// </summary>
        public TypeSettings TypeSettings { get; }

        /// <summary>
        ///     The scope type settings
        /// </summary>
        public TypeSettings MemberSettings { get; }

        /// <summary>
        ///     The scope type settings
        /// </summary>
        public IConverterSettings UserSettings { get; }

        /// <inheritdoc />
        public ScopedConverterSettings(IConverterSettings userSettings, TypeSettings typeSettings, TypeSettings memberSettings) {
            TypeSettings = typeSettings;
            MemberSettings = memberSettings;
            UserSettings = userSettings;
        }

        /// <inheritdoc />
        public INamingStrategy NamingStrategy => MemberSettings?.NamingStrategy ?? TypeSettings?.NamingStrategy ?? UserSettings.NamingStrategy;

        /// <inheritdoc />
        public string TimeFormat => MemberSettings?.TimeFormat ?? TypeSettings?.TimeFormat ?? UserSettings.TimeFormat;

        /// <inheritdoc />
        public string NumberFormat => MemberSettings?.NumberFormat ?? TypeSettings?.NumberFormat ?? UserSettings.NumberFormat;

        /// <inheritdoc />
        public string GuidFormat => MemberSettings?.GuidFormat ?? TypeSettings?.GuidFormat ?? UserSettings.GuidFormat;

        /// <inheritdoc />
        public string EnumFormat => MemberSettings?.EnumFormat ?? TypeSettings?.EnumFormat ?? UserSettings.EnumFormat;

        /// <inheritdoc />
        public EmitMode EmitMode {
            get {
                if(MemberSettings != null && MemberSettings.EmitMode != EmitMode.Inherit)
                    return MemberSettings.EmitMode;

                if(TypeSettings != null && TypeSettings.EmitMode != EmitMode.Inherit)
                    return TypeSettings.EmitMode;

                if(UserSettings != null && UserSettings.EmitMode != EmitMode.Inherit)
                    return UserSettings.EmitMode;

                return EmitMode.Emit;
            }
        }

        /// <inheritdoc />
        public ScopedConverterSettings ChildSettings
        {
            get
            {
                if (TypeSettings == null && MemberSettings == null)
                    return this;
                return new ScopedConverterSettings(UserSettings, TypeSettings?.ChildSettings, MemberSettings?.ChildSettings);
            }
        }

        /// <inheritdoc />
        public IFormatProvider FormatProvider {
            get => UserSettings.FormatProvider;
            set => UserSettings.FormatProvider = value;
        }

        /// <inheritdoc />
        public IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null)
        {
            if ((typeSettings != null && typeSettings.HasOverrides) || (memberSettings != null && memberSettings.HasOverrides))
                return new ScopedConverterSettings(UserSettings, memberSettings, typeSettings);
            return UserSettings;
        }


        public virtual bool ShouldEmitValue(object value) {
            return ConverterSettings.ShouldEmitValue(value, EmitMode);
        }
    }

    public class ConverterSettings : IConverterSettings {
        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings Default => new ConverterSettings();

        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings JsonDefault => JsonFormatterSettings.Default;

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        public INamingStrategy NamingStrategy { get; set; } = NamingStrategies.CamelCase;

        /// <summary>
        ///     The time format (o by default)
        /// </summary>
        public string TimeFormat { get; set; } = "o";

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        public string NumberFormat { get; set; } = "G";

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        public string GuidFormat { get; set; } = "D";

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        public string EnumFormat { get; set; } = "D";

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        public EmitMode EmitMode { get; set; } = EmitMode.Emit;

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public INodeConverterResolver Resolver { get; set; } = Nodes.DefaultResolver;

        public virtual IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null) {
            if ((typeSettings != null && typeSettings.HasOverrides) || (memberSettings != null && memberSettings.HasOverrides))
                return new ScopedConverterSettings(this, memberSettings, typeSettings);
            return this;
        }

        public virtual bool ShouldEmitValue(object value) {
            return ShouldEmitValue(value, EmitMode);
        }

        public static bool ShouldEmitValue(object value, EmitMode emitMode) {
            if(value == null)
                return emitMode == EmitMode.Emit;

            var type = value.GetType();
            return (type.IsValueType == false) || (emitMode != EmitMode.IgnoreDefault) || (Equals(type.GetDefaultValue(), value) == false);
        }
    }
}
