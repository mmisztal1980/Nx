using Ninject;
using Nx.Extensions;
using System;

namespace Nx.Cloud.Tests
{
    public abstract class TestFixtureBase
    {
        public IKernel Kernel { get; private set; }

        public TestFixtureBase()
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        protected virtual void OnDisposing()
        {
        }
    }
}
