using MetroLog;
using MetroLog.Targets;
using System;

namespace Nx.Logging
{
    /// <summary>
    /// Logger wrapper for RT using MetroLog based on NLog
    /// </summary>
    public class MetroLogger : ILogger
    {
        private MetroLog.ILogger Logger;
        private MetroLog.LoggingConfiguration LogConfig = new LoggingConfiguration();

        public MetroLogger()
        {
            LogConfig.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());
            Logger = LogManagerFactory.DefaultLogManager.GetLogger<MetroLogger>(LogConfig);
        }

        public MetroLogger(string loggerName)
        {
            LogConfig.AddTarget(LogLevel.Trace, LogLevel.Fatal, new DebugTarget());
            Logger = LogManagerFactory.DefaultLogManager.GetLogger(loggerName, LogConfig);
        }

        ~MetroLogger()
        {
            Dispose(false);
        }

        public void Debug(string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            Logger.Info(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            Logger.Warn(message, args);
        }

        public void Error(string message, params object[] args)
        {
            Logger.Error(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            Logger.Error(message, args);
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
                this.Logger = null;
            }
        }
    }
}