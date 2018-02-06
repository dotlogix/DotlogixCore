using System;
using System.Collections.Generic;

namespace DotLogix.Core.Reflection.Projections {
    public delegate IEnumerable<IProjection> CreateProjectionsCallback(Type left, Type right);
}