using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Tracking.Snapshots {
    /// <inheritdoc />
    public class DiffSnapshotFactory : ISnapshotFactory {
        private readonly DynamicAccessor[] _accessors;

        /// <summary>
        /// Create a new instance of <see cref="DiffSnapshotFactory"/>
        /// </summary>
        /// <param name="accessors"></param>
        public DiffSnapshotFactory(IEnumerable<DynamicAccessor> accessors) {
            _accessors = accessors.ToArray();
        }

        /// <inheritdoc />
        public ISnapshot CreateSnapshot(object target) {
            return new DiffSnapshot(target, _accessors);
        }
    }
}