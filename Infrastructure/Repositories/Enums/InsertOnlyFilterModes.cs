using System;

namespace DotLogix.Architecture.Infrastructure.Repositories.Enums {
    public enum InsertOnlyFilterModes {
        Active = 1 << 0,
        InActive = 1 << 1,
        All = Active | InActive
    }
}