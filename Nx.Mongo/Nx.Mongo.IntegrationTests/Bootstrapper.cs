using Ninject;
using Nx.Bootstrappers;
using Nx.Kernel;
using System.Configuration;

namespace Nx.Mongo.IntegrationTests
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void ConfigureContainer()
        {
            Kernel.RegisterTypeIfMissing<IMongoContextFactory, MongoContextFactory>(false);
            Kernel.RegisterTypeIfMissing<IMongoEntityRepository, MongoEntityRepository>(false);
        }

        protected override Ninject.IKernel CreateContainer()
        {
            return new StandardKernel();
        }

        private void RegisterConfigurationSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            var section = GetConfigurationSection<T>(sectionName);
            if (section != null)
            {
                Kernel.RegisterInstance<T>(section);
            }
        }

        private T GetConfigurationSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}