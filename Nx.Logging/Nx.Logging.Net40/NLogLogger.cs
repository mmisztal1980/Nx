using NLog;
using System;

namespace Nx.Logging
{
    public class NLogLogger : ILogger
    {
        private Logger Logger;

        public void Debug(string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            Logger.Error(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            Logger.Fatal(message, args);
        }

        public void Info(string message, params object[] args)
        {
            Logger.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            Logger.Warn(message, args);
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
                Logger = null;
            }
        }

        public NLogLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(string loggerName)
        {
            Logger = LogManager.GetLogger(loggerName);
        }

        ~NLogLogger()
        {
            Dispose(false);
        }
    }
}
