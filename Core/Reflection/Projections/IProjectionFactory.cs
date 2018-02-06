using System;
using System.Collections.Generic;

namespace DotLogix.Core.Reflection.Projections {
    public interface IProjectionFactory {
        IEnumerable<IProjection> CreateProjections(Type left, Type right);
    }
}