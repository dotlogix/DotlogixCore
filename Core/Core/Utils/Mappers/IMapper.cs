using System;

namespace DotLogix.Core.Utils.Mappers; 

/// <summary>
/// An interface representation type mappers
/// </summary>
public interface IMapper<TSource, TTarget> {
    /// <summary>
    /// The source type
    /// </summary>
    Type SourceType { get; }
        
    /// <summary>
    /// The target type
    /// </summary>
    Type TargetType { get; }
        
    /// <summary>
    /// Maps a source type to a target type
    /// </summary>
    void Map(TSource source, TTarget target);
}