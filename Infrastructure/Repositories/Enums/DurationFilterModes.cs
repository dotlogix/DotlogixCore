using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLogix.Architecture.Infrastructure.Repositories.Enums
{
    [Flags]
    public enum DurationFilterModes {
        Current = 1 << 0,
        InFuture = 1 << 1,
        Outdated = 1 << 2,
        All = Current | InFuture | Outdated
    }
}
