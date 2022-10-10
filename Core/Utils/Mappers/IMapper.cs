namespace DotLogix.Core.Utils.Mappers {
    /// <summary>
    /// An interface representation type mappers
    /// </summary>
    public interface IMapper<TSource, TTarget> {
        /// <summary>
        /// Maps a source type to a target type
        /// </summary>
        void Map(TSource source, TTarget target);
    }
}