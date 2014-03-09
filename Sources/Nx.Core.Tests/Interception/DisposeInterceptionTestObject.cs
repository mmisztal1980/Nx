using Nx.Interception;
using System;
using System.IO;

namespace Nx.Core.Tests.Interception
{
    public class DisposeInterceptionTestObject : IDisposable
    {
        public virtual bool Disposed { get; private set; }

        public DisposeInterceptionTestObject()
        {
            Disposed = false;
            _disposableField = new MemoryStream();
            DisposableProperty = new MemoryStream();
        }

        [Dispose]
        private Stream _disposableField;

        public virtual Stream GetDisposableField()
        {
            return _disposableField;
        }

        [Dispose]
        public virtual Stream DisposableProperty { get; private set; }

        [ExtendedDispose]
        public virtual void Dispose()
        {
            Disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}