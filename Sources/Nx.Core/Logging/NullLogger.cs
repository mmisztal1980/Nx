using System;

namespace Nx.Logging
{
    public sealed class NullLogger : ILogger
    {
        private const string LayoutFormat = "[{0}] [{1}] {2}";
        private readonly string _loggerName;

        public NullLogger(string loggerName)
        {
            _loggerName = loggerName;
        }

        ~NullLogger()
        {
        }

        public void Debug(string message, params object[] args)
        {
            Write("DEBUG", message, args);
        }

        public void DebugException(string message, Exception exception)
        {
            Debug(message, exception.ToString());
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Error(string message, params object[] args)
        {
            Write("ERROR", message, args);
        }

        public void ErrorException(string message, Exception exception)
        {
            Error(message, exception.ToString());
        }

        public void Fatal(string message, params object[] args)
        {
            Write("FATAL", message, args);
        }

        public void FatalException(string message, Exception exception)
        {
            Fatal(message, exception.ToString());
        }

        public void Info(string message, params object[] args)
        {
            Write("INFO", message, args);
        }

        public void InfoException(string message, Exception exception)
        {
            Info(message, exception.ToString());
        }

        public void Trace(string message, params object[] args)
        {
            Write("TRACE", message, args);
        }

        public void TraceException(string message, Exception exception)
        {
            Trace(message, exception.ToString());
        }

        public void Warning(string message, params object[] args)
        {
            Write("WARNING", message, args);
        }

        public void WarningException(string message, Exception exception)
        {
            Warning(message, exception.ToString());
        }

        private void Write(string mode, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            System.Diagnostics.Debug.WriteLine(LayoutFormat, _loggerName, mode, msg);
        }
    }
}