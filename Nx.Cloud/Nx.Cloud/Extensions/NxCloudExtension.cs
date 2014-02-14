using Ninject;
using Nx.Bootstrappers;
using Nx.Cloud.Blobs;
using Nx.Cloud.Concurrency;

namespace Nx.Extensions
{
    public class NxCloudExtension : IBootstrapperExtension
    {
        public void Extend(IKernel kernel)
        {
            kernel.Bind<IBlobRepository<CloudLockBlobData>>().To<CloudLockBlobRepository>();
        }

        public string Name
        {
            get { return "Nx.Cloud Extension"; }
        }
    }
}
