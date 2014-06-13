using System;

namespace Nx
{
    public abstract class Disposable : IDisposable
    {
        protected bool IsDisposed;

        protected Disposable()
        {
        }

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            try
            {
                Dispose(true);
            }
            catch
            {
            }
            finally
            {
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}