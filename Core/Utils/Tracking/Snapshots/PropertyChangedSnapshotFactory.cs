using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Tracking.Snapshots {
    /// <inheritdoc />
    public class PropertyChangedSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        /// <summary>
        /// Create a new instance of <see cref="PropertyChangedSnapshotFactory"/>
        /// </summary>
        /// <param name="accessors"></param>
        public PropertyChangedSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        /// <inheritdoc />
        public ISnapshot CreateSnapshot(object target) {
            var npc = (INotifyPropertyChanged)target;
            return new PropertyChangedSnapshot(npc, _accessors);
        }
    }
}