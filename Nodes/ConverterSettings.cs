using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Nodes {
    public class ScopedConverterSettings : ConverterSettings {
        /// <summary>
        ///     The scope type settings
        /// </summary>
        public IConverterSettings TypeSettings { get; protected set; }

        /// <summary>
        ///     The scope type settings
        /// </summary>
        public IConverterSettings MemberSettings { get; protected set; }

        /// <summary>
        ///     The scope type settings
        /// </summary>
        public IConverterSettings UserSettings { get; protected set; }

        /// <inheritdoc />
        public override IConverterSettings ChildSettings {
            get {
                var childTypeSettings = TypeSettings?.ChildSettings;
                var childMemberSettings = MemberSettings?.ChildSettings;
                return new ScopedConverterSettings(UserSettings, childTypeSettings, childMemberSettings);
            }
            set => throw new NotSupportedException();
        }


        /// <inheritdoc />
        public ScopedConverterSettings(IConverterSettings userSettings, IConverterSettings typeSettings = null, IConverterSettings memberSettings = null) : base(userSettings?.Settings) {
            TypeSettings = typeSettings;
            MemberSettings = memberSettings;
            UserSettings = userSettings;
            Settings = null;
        }

        /// <inheritdoc />
        protected override T GetWithMemberName<T>(T defaultValue = default, string memberName = null) {
            if((MemberSettings != null) && MemberSettings.Settings.TryGet(memberName, out T value))
                return value;

            if((TypeSettings != null) && TypeSettings.Settings.TryGet(memberName, out value))
                return value;

            if((UserSettings != null) && UserSettings.Settings.TryGet(memberName, out value))
                return value;
            return defaultValue;
        }

        /// <inheritdoc />
        protected override void SetWithMemberName(object value, string memberName = null) {
            throw new NotSupportedException("You can not override a setting on scoped settings");
        }

        /// <inheritdoc />
        public override IConverterSettings GetScoped(TypeSettings typeSettings = null, MemberSettings memberSettings = null) {
            return new ScopedConverterSettings(UserSettings, typeSettings ?? TypeSettings, memberSettings ?? MemberSettings);
        }
    }

    public class ConverterSettings : IConverterSettings {
        /// <summary>
        ///     The inner settings
        /// </summary>
        public ISettings Settings { get; protected set; }
        IReadOnlySettings IReadOnlyConverterSettings.Settings => Settings;

        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings Default => new ConverterSettings();

        /// <summary>
        ///     The default settings
        /// </summary>
        public static ConverterSettings JsonDefault => new ConverterSettings {Resolver = Nodes.DefaultJsonResolver};

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider {
            get => GetWithMemberName((IFormatProvider)CultureInfo.InvariantCulture);
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public INodeConverterResolver Resolver {
            get => GetWithMemberName(Nodes.DefaultResolver);
            set => SetWithMemberName(value);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="Settings" />
        /// </summary>
        public ConverterSettings() : this(null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="Settings" />
        /// </summary>
        public ConverterSettings(ISettings settings) {
            Settings = settings ?? new Settings(StringComparer.Ordinal);
        }

        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        public INamingStrategy NamingStrategy {
            get => GetWithMemberName(NamingStrategies.CamelCase);
            set => SetWithMemberName(value);
        }

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

        /// <inheritdoc />
        public virtual IConverterSettings ChildSettings {
            get => GetWithMemberName<IConverterSettings>();
            set => SetWithMemberName(value);
        }


        /// <inheritdoc />
        IReadOnlyConverterSettings IReadOnlyConverterSettings.ChildSettings => ChildSettings;

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected virtual void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
            Settings.Set(memberName, value);
        }

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected virtual T GetWithMemberName<T>(T defaultValue = default, [CallerMemberName] string memberName = null) {
            return Settings.Get(memberName, defaultValue);
        }
        
        public virtual IConverterSettings GetScoped(TypeSettings typeSettings = null, MemberSettings memberSettings = null) {
            return new ScopedConverterSettings(this, typeSettings, memberSettings);
        }

        public virtual bool ShouldEmitValue(object value) {
            var emitMode = EmitMode;
            if (value == null)
                return emitMode == EmitMode.Emit;

            var type = value.GetType();
            return (type.IsValueType == false) || (emitMode != EmitMode.IgnoreDefault) || (Equals(type.GetDefaultValue(), value) == false);
        }
    }
}
