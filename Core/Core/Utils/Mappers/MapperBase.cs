using System;

namespace DotLogix.Core.Utils.Mappers; 

public abstract class MapperBase<TSource, TTarget> : IMapper<TSource, TTarget> {
    public Type SourceType { get; }
    public Type TargetType { get; }

    protected MapperBase() {
        SourceType = typeof(TSource);
        TargetType = typeof(TTarget);
    }
        
    protected MapperBase(Type sourceType, Type targetType) {
        SourceType = sourceType ?? typeof(TSource);
        TargetType = targetType ?? typeof(TTarget);
    }

    public abstract void Map(TSource source, TTarget target);
}