using System.Configuration;

namespace Nx.Cloud.Configuration
{
    public class CloudConfigurationSection : ConfigurationSection
    {
        private const string StorageConnectionStringSettingName = "storageConnectionString";

        [ConfigurationProperty("storageConnectionString", IsRequired = true)]
        public string StorageConnectionString
        {
            get { return (string)base[StorageConnectionStringSettingName]; }
            set { base[StorageConnectionStringSettingName] = value; }
        }
    }
}
