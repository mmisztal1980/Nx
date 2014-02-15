using Ninject;
using Ninject.Modules;
using Nx.Bootstrappers;
using Nx.Cloud.Blobs;
using Nx.Cloud.Configuration;
using Nx.Cloud.Queues;
using Nx.Cloud.Tables;
using Nx.Cloud.Tests.Blobs;
using Nx.Cloud.Tests.Queues;
using Nx.Cloud.Tests.Tables;
using Nx.Kernel;
using System.Configuration;

namespace Nx.Cloud.Tests
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper(params INinjectModule[] modules)
            : base(modules)
        { }

        protected override void ConfigureContainer()
        {
            this.Kernel.Bind<ICloudConfiguration>()
                .ToConstant(CloudConfiguration.Instance)
                .OnActivation((ctx, config) => config.Initialize(() => false, () => false));

            this.Kernel.RegisterTypeIfMissing<IBlobRepository<TestBlobData>, TestBlobDataRepository>(false);
            this.Kernel.RegisterTypeIfMissing<ITableRepository<TestTableData>, TestTableRepository>(false);
            this.Kernel.RegisterTypeIfMissing<IQueueService<TestQueueItem>, TestQueueService>(false);
        }

        protected override IKernel CreateContainer()
        {
            return new StandardKernel();
        }

        private T GetConfiguration<T>(string name)
            where T : ConfigurationSection
        {
            return ConfigurationManager.GetSection(name) as T;
        }

        private void ParseConfiguration()
        {
            var cloudConfig = GetConfiguration<CloudConfigurationSection>("cloudConfiguration");
            if (cloudConfig != null)
            {
                this.Kernel.RegisterInstance(cloudConfig);
            }
        }
    }
}