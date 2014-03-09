using Ninject.Extensions.Interception;
using Nx.Logging;
using System;
using System.Diagnostics;

namespace Nx.Interception
{
    public class MeasureExecutionTimeInterceptor : SimpleInterceptor, IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;

        public MeasureExecutionTimeInterceptor(ILogFactory logFactory, string loggerName)
        {
            _stopwatch = new Stopwatch();
            _logger = logFactory.CreateLogger(loggerName);
        }

        ~MeasureExecutionTimeInterceptor()
        {
            Dispose(false);
        }

        protected override void BeforeInvoke(IInvocation invocation)
        {
            _logger.Debug("Beginning measurement of {0}", invocation.Request.Method.Name);
            _stopwatch.Start();
        }

        protected override void AfterInvoke(IInvocation invocation)
        {
            _stopwatch.Stop();
            _logger.Debug("Execution of {0} completed in {1} [ms]",
                invocation.Request.Method.Name,
                _stopwatch.ElapsedMilliseconds);
            _stopwatch.Reset();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logger.Dispose();
            }
        }
    }
}