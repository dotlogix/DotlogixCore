using System;
using DotLogix.Core.Utils.Naming;

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
}