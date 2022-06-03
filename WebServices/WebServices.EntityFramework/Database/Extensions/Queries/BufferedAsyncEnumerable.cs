using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions
{
    public class BufferedAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly Func<CancellationToken, Task<IReadOnlyList<T>>> _getResultsAsync;

        public BufferedAsyncEnumerable(Func<CancellationToken, Task<IReadOnlyList<T>>> getResultsAsync)
        {
            _getResultsAsync = getResultsAsync;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return new BufferedAsyncEnumerator(_getResultsAsync, cancellationToken);
        }

        private class BufferedAsyncEnumerator : IAsyncEnumerator<T>
        {
            private readonly Func<CancellationToken, Task<IReadOnlyList<T>>> _getResultsAsync;
            private readonly CancellationToken _cancellationToken;
            private int _index;
            private IReadOnlyList<T> _results;

            public BufferedAsyncEnumerator(
                Func<CancellationToken, Task<IReadOnlyList<T>>> getResultsAsync,
                CancellationToken cancellationToken
            )
            {
                _getResultsAsync = getResultsAsync;
                _cancellationToken = cancellationToken;
            }


            public T Current { get; private set; }

            public ValueTask<bool> MoveNextAsync()
            {
                if (_results != null)
                {
                    return new ValueTask<bool>(MoveNext());
                }

                var task = GetResultsAndMoveNextAsync();
                return new ValueTask<bool>(task);
            }

            public ValueTask DisposeAsync() => new();

            private async Task<bool> GetResultsAndMoveNextAsync()
            {
                _results = await _getResultsAsync(_cancellationToken);
                return MoveNext();
            }

            private bool MoveNext()
            {
                if (_index < _results.Count)
                {
                    Current = _results[_index];
                    _index++;
                    return true;
                }
                return false;
            }
        }
    }
}