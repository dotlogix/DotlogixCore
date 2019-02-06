using System.Collections.Generic;

namespace DotLogix.Core.Patterns {
    public interface IPipeline<TIn, TOut> : IPipelineStep<TIn, TOut>, IList<IPipelineStep<TIn, TOut>>{
        
    }
}