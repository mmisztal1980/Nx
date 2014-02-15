using Ninject;
using Nx.Extensions;
using System;

namespace Nx.Cloud.Tests
{
    public abstract class TestFixtureBase
    {
        protected TestFixtureBase()
        {
            using (var bootstrapper = new Bootstrapper()
                .ExtendBy<NxLoggingExtension>()
                .ExtendBy<NxCloudExtension>())
            {
                Kernel = bootstrapper.Run();
            }
        }

        ~TestFixtureBase()
        {
            Dispose(false);
        }

        public IKernel Kernel { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposing()
        {
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnDisposing();

                if (this.Kernel != null)
                {
                    this.Kernel.Dispose();
                }
            }
        }
    }
}