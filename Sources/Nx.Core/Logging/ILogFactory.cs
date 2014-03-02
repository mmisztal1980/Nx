using System;

namespace Nx.Logging
{
    public interface ILogFactory : IDisposable
    {
        ILogger CreateLogger(string loggerName);
        ILogger CreateLogger(object loggerOwner);
    }
}
