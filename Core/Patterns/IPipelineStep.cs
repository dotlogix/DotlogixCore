using System.Threading.Tasks;

namespace DotLogix.Core.Patterns {
    public interface IPipelineStep<TIn, TOut> {
        IPipelineStep<TIn, TOut> NextStep { get; set; }
        Task<Optional<TOut>> Execute(TIn value);
    }
}