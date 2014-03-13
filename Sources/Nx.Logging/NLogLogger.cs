using NLog;
using System;

namespace Nx.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger _logger;

        public void Trace(string message, params object[] args)
        {
            _logger.Trace(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _logger.Warn(message, args);
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
                _logger = null;
            }
        }

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        ~NLogLogger()
        {
            Dispose(false);
        }

        public void TraceException(string message, Exception exception)
        {
            _logger.TraceException(message, exception);
        }

        public void DebugException(string message, Exception exception)
        {
            _logger.DebugException(message, exception);
        }

        public void InfoException(string message, Exception exception)
        {
            _logger.InfoException(message, exception);
        }

        public void WarningException(string message, Exception exception)
        {
            _logger.WarnException(message, exception);
        }

        public void ErrorException(string message, Exception exception)
        {
            _logger.ErrorException(message, exception);
        }

        public void FatalException(string message, Exception exception)
        {
            _logger.FatalException(message, exception);
        }
    }
}