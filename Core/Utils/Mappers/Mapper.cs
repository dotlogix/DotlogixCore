using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Utils.Mappers {
    public class Mapper<TSource, TTarget> : IMapper<TSource, TTarget> {
        private readonly List<IMapper<TSource, TTarget>> _valueMappers;

        public Mapper(params IMapper<TSource, TTarget>[] valueMappers) : this(valueMappers.AsEnumerable()) { }

        public Mapper(IEnumerable<IMapper<TSource, TTarget>> valueMappers) {
            _valueMappers = new List<IMapper<TSource, TTarget>>(valueMappers);
        }

        public void Map(TSource source, TTarget target) {
            foreach(var valueMapper in _valueMappers)
                valueMapper.Map(source, target);
        }
    }
}