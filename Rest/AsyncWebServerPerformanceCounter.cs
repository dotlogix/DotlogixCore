using System;
using System.Diagnostics;

namespace DotLogix.Core.Rest {
    public class AsyncWebServerPerformanceCounter {
        private readonly Stopwatch _watch = new Stopwatch();
        public AsyncWebRequestMetrics Metrics { get; } = new AsyncWebRequestMetrics();


        public void BeginRequest() {
            Metrics.FromUtc = DateTime.UtcNow;
        }

        public void EndRequest() {
            Metrics.UntilUtc = DateTime.UtcNow;
        }

        public void BeginExecute() {
            _watch.Restart();
        }

        public void EndExecute() {
            _watch.Stop();
            Metrics.ExecutionDuration = _watch.Elapsed;
        }

        public void BeginWriteResponse() {
            _watch.Restart();
        }

        public void EndWriteResponse() {
            _watch.Stop();
            Metrics.ExecutionDuration = _watch.Elapsed;
        }
        
        public void BeginPreProcessing() {

            _watch.Restart();
        }

        public void EndPreProcessing() {

            _watch.Stop();
            Metrics.PreProcessingDuration = _watch.Elapsed;
        }
        public void BeginPostProcessing() {

            _watch.Restart();
        }

        public void EndPostProcessing() {

            _watch.Stop();
            Metrics.PostProcessingDuration = _watch.Elapsed;
        }
    }
}