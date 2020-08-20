using System;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes {
    public class ScopedConverterSettings : IReadOnlyConverterSettings
    {
        private readonly IReadOnlyConverterSettings _typeSettings;
        private readonly IReadOnlyConverterSettings _memberSettings;
        private readonly IConverterSettings _userSettings;

        /// <inheritdoc />
        public ScopedConverterSettings(IConverterSettings userSettings, IReadOnlyConverterSettings typeSettings, IReadOnlyConverterSettings memberSettings) {
            _typeSettings = typeSettings;
            _memberSettings = memberSettings;
            _userSettings = userSettings;
        }

        /// <inheritdoc />
        public INamingStrategy NamingStrategy => _memberSettings?.NamingStrategy ?? _typeSettings?.NamingStrategy ?? _userSettings.NamingStrategy;

        /// <inheritdoc />
        public string TimeFormat => _memberSettings?.TimeFormat ?? _typeSettings?.TimeFormat ?? _userSettings.TimeFormat;

        /// <inheritdoc />
        public string NumberFormat => _memberSettings?.NumberFormat ?? _typeSettings?.NumberFormat ?? _userSettings.NumberFormat;

        /// <inheritdoc />
        public string GuidFormat => _memberSettings?.GuidFormat ?? _typeSettings?.GuidFormat ?? _userSettings.GuidFormat;

        /// <inheritdoc />
        public string EnumFormat => _memberSettings?.EnumFormat ?? _typeSettings?.EnumFormat ?? _userSettings.EnumFormat;

        /// <inheritdoc />
        public EmitMode EmitMode {
            get {
                if(_memberSettings != null && _memberSettings.EmitMode != EmitMode.Inherit)
                    return _memberSettings.EmitMode;

                if(_typeSettings != null && _typeSettings.EmitMode != EmitMode.Inherit)
                    return _typeSettings.EmitMode;

                if(_userSettings != null && _userSettings.EmitMode != EmitMode.Inherit)
                    return _userSettings.EmitMode;

                return EmitMode.Emit;
            }
        }

        /// <inheritdoc />
        public IReadOnlyConverterSettings GetScoped(TypeSettings typeSettings = null, TypeSettings memberSettings = null)
        {
            var typeOverrides = typeSettings?.Overrides;
            var memberOverrides = memberSettings?.Overrides;
            if (typeOverrides != null || memberOverrides != null)
                return new ScopedConverterSettings(_userSettings, typeSettings?.Overrides, memberSettings?.Overrides);
            return _userSettings;
        }


        public virtual bool ShouldEmitValue(object value) {
            return ConverterSettings.ShouldEmitValue(value, EmitMode);
        }
    }
}