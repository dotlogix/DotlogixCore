using System;

namespace DotLogix.Core.Utils.Mappers {
    public interface IValueMapper<TSource, TTarget> : IMapper<TSource, TTarget> {
        /// <summary>
        /// The source value type
        /// </summary>
        Type SourceValueType { get; }
        
        /// <summary>
        /// The target value type
        /// </summary>
        Type TargetValueType { get; }
    }
}