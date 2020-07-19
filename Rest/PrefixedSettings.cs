using System.Runtime.CompilerServices;
using DotLogix.Core.Utils;

namespace DotLogix.Core.Rest {
    public class PrefixedSettings {
        public PrefixedSettings() {
            Settings = new Settings();
        }

        public PrefixedSettings(ISettings settings, string prefix = null) {
            Prefix = prefix;
            Settings = settings ?? new Settings();
        }

        public string Prefix { get; protected set; }
        public ISettings Settings { get; protected set; }

        #region Ops
        public virtual Optional<T> GetOptional<T>(string name) {
            return Settings.Get<T>(ModifyName(name));
        }

        public T Get<T>(string name, T defaultValue) {
            return GetOptional<T>(name).GetValueOrDefault(defaultValue);
        }

        public virtual void Set(string name, object value) {
            Settings.Set(ModifyName(name), value);
        }

        protected virtual string ModifyName(string name) {
            return string.Concat(Prefix, name);
        }

        #endregion

        #region Helper

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
            Set(memberName, value);
        }

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected T GetWithMemberName<T>(T defaultValue = default, [CallerMemberName] string memberName = null) {
            return Get(memberName, defaultValue);
        }

        /// <summary>
        ///     If called by a class member the member name can be omitted
        /// </summary>
        protected Optional<T> GetOptionalWithMemberName<T>([CallerMemberName] string memberName = null) {
            return GetOptional<T>(memberName);
        }

        #endregion
    }
}