using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils {
    public class LayeredSettings : CascadingSettings, ILayeredSettings {
        /// <summary>
        /// The source settings stack
        /// </summary>
        protected new Stack<IReadOnlySettings> Settings { get; }

        /// <inheritdoc />
        public IEnumerable<IReadOnlySettings> Layers => Settings;

        /// <inheritdoc />
        public IReadOnlySettings CurrentLayer => PeekLayer();

        /// <inheritdoc />
        public LayeredSettings() : this(default(IEqualityComparer<string>)) {
            
        }
        
        /// <inheritdoc />
        public LayeredSettings(IEqualityComparer<string> settingsKeyComparer) : base(new Stack<IReadOnlySettings>(), settingsKeyComparer ?? StringComparer.Ordinal) {
            Settings = (Stack<IReadOnlySettings>)base.Settings;
        }
        
        /// <inheritdoc />
        public LayeredSettings(params IReadOnlySettings[] initialStack) : this(initialStack.AsEnumerable()) {
        }
        
        /// <inheritdoc />
        public LayeredSettings(IEnumerable<IReadOnlySettings> initialStack, IEqualityComparer<string> settingsKeyComparer = null) : base(new Stack<IReadOnlySettings>(initialStack), settingsKeyComparer ?? StringComparer.Ordinal) {
            Settings = (Stack<IReadOnlySettings>)base.Settings;
        }

        /// <inheritdoc />
        public override IReadOnlySettings Clone() {
            return new LayeredSettings(Settings, SettingsKeyComparer);
        }


        /// <inheritdoc />
        public ISettings PushLayer() {
            var settings = new Settings(SettingsKeyComparer);
            Settings.Push(settings);
            return settings;
        }

        /// <inheritdoc />
        public virtual IReadOnlySettings PopLayer() {
            return Settings.Pop();
        }

        /// <inheritdoc />
        public virtual IReadOnlySettings PeekLayer() {
            return Settings.Peek();
        }
    }
}