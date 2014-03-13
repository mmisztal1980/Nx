using System;

namespace Nx.Logging
{
    public interface ILogger : IDisposable
    {
        void Trace(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(string message, params object[] args);

        void TraceException(string message, Exception exception);

        void DebugException(string message, Exception exception);

        void InfoException(string message, Exception exception);

        void WarningException(string message, Exception exception);

        void ErrorException(string message, Exception exception);

        void FatalException(string message, Exception exception);
    }
}