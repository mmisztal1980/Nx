using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Configuration;

namespace Nx.Cloud.Configuration
{
    /// <summary>
    /// The ICloudConfiguration Singleton Implementation
    /// <remarks>
    /// 1. Requires Initialization
    /// 2. When running in Azure, requires the presence of "StorageConnectionString" setting with the connection string present
    /// 3. When running outside of Azure, requires the presence of "cloudConfiguration" configuration section
    /// </remarks>
    /// </summary>
    public class CloudConfiguration : Singleton<CloudConfiguration>, ICloudConfiguration
    {
        private const string CloudConfigurationSectionName = "cloudConfiguration";
        private const string CloudConfigurationManagerConnectionStringSettingName = "StorageConnectionString";
        private const string CloudConfigurationUninitializedError = "CloudConfiguration uninitialized";
        private const string CloudConfigurationAlreadyInitializedError = "CloudConfiguration object already initialized";
        private const string CloudConfigurationConfigSectionMissingError = "The cloudConfiguration configuration section is missing in the configuration file";

        private Func<bool> _isRunningInAzure;
        private Func<bool> _isRunningInAzureEmulator;
        private bool _Initialized = false;
        private string _ConnectionString;

        public CloudConfiguration()
        {
        }

        public void Initialize(Func<bool> runningInAzure, Func<bool> runningInEmulator)
        {
            if (!_Initialized)
            {
                Condition.ArgumentNotNull(runningInAzure, "runningInAzure");
                Condition.ArgumentNotNull(runningInEmulator, "runningInEmulator");

                _isRunningInAzure = runningInAzure;
                _isRunningInAzureEmulator = runningInEmulator;

                _Initialized = true;

                // Determine the ConnectionString
                if (IsRunningInAzure)
                {
                    _ConnectionString = CloudConfigurationManager.GetSetting(CloudConfigurationManagerConnectionStringSettingName);
                }
                else
                {
                    CloudConfigurationSection configSection = GetConfigurationSection<CloudConfigurationSection>(CloudConfigurationSectionName);
                    Condition.Require<ConfigurationException>(configSection != null, CloudConfigurationConfigSectionMissingError);
                    _ConnectionString = configSection.StorageConnectionString;
                }
            }
        }

        public bool Initialized
        {
            get
            {
                return _Initialized;
            }
        }

        public bool IsRunningInAzure
        {
            get
            {
                Condition.Require<InvalidOperationException>(_Initialized, CloudConfigurationUninitializedError);
                return _isRunningInAzure();
            }
        }

        public bool IsRunningInEmulator
        {
            get
            {
                Condition.Require<InvalidOperationException>(_Initialized, CloudConfigurationUninitializedError);
                return _isRunningInAzure() && _isRunningInAzureEmulator();
            }
        }

        public string ConnectionString
        {
            get
            {
                Condition.Require<InvalidOperationException>(_Initialized, CloudConfigurationUninitializedError);
                return _ConnectionString;
            }
        }

        private CloudStorageAccount _StorageAccount;
        public CloudStorageAccount StorageAccount
        {
            get
            {
                Condition.Require<InvalidOperationException>(_Initialized, CloudConfigurationUninitializedError);

                if (_StorageAccount == null)
                {
                    _StorageAccount = CloudStorageAccount.Parse(ConnectionString);
                }

                return _StorageAccount;
            }
        }

        private IRetryPolicy _GlobalRetryPolicy;
        public IRetryPolicy GlobalRetryPolicy
        {
            get
            {
                Condition.Require<InvalidOperationException>(_Initialized, CloudConfigurationUninitializedError);

                if (_GlobalRetryPolicy == null)
                {
                    _GlobalRetryPolicy = new LinearRetry(new TimeSpan(0, 0, 0, 0, 500), 5);
                }

                return _GlobalRetryPolicy;
            }
        }

        private T GetConfigurationSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}
