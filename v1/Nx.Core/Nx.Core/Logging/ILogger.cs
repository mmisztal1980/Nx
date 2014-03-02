using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nx.Logging
{
    public interface ILogger : IDisposable
    {
        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(string message, params object[] args);
    }
}
