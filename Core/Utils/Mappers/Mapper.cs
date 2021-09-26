using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils.Mappers {
    public class Mapper<TSource, TTarget> : MapperBase<TSource, TTarget> {
        protected List<IMapper<TSource, TTarget>> InternalMappers { get; } = new List<IMapper<TSource, TTarget>>();
        public IReadOnlyCollection<IMapper<TSource, TTarget>> Mappers => InternalMappers;

        public Mapper(params IMapper<TSource, TTarget>[] valueMappers) : this(valueMappers.AsEnumerable()) { }

        public Mapper(IEnumerable<IMapper<TSource, TTarget>> valueMappers) {
            InternalMappers.AddRange(valueMappers);
        }

        /// <inheritdoc />
        public override void Map(TSource source, TTarget target) {
            foreach(var valueMapper in InternalMappers) {
                valueMapper.Map(source, target);
            }
        }
    }
}