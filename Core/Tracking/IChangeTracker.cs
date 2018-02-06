using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Tracking.Entries;

namespace DotLogix.Core.Tracking
{
    public interface IChangeTracker {
        IEnumerable<IChangeTrackingEntry> Entries { get; }
        void MarkAsAdded(object target);
        void MarkAsDeleted(object target);
        void MarkAsModified(object target);
        void Attach(object target);
        void Detach(object target);
        IChangeTrackingEntry Entry(object target);
    }
}
