using System;

namespace Nx.Logging
{
    public sealed class NullLogger : ILogger
    {
        private string loggerName;
        private const string layoutFormat = "[{0}] [{1}] {2}";

        public NullLogger(string loggerName)
        {
            this.loggerName = loggerName;
        }

        public void Debug(string message, params object[] args)
        {
            Write("DEBUG", message, args);
        }

        public void Info(string message, params object[] args)
        {
            Write("INFO", message, args);
        }

        public void Warning(string message, params object[] args)
        {
            Write("WARNING", message, args);
        }

        public void Error(string message, params object[] args)
        {
            Write("ERROR", message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            Write("FATAL", message, args);
        }

        private void Write(string mode, string message, params object[] args)
        {
            string msg = string.Format(message, args);
            System.Diagnostics.Debug.WriteLine(layoutFormat, loggerName, mode, msg);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
