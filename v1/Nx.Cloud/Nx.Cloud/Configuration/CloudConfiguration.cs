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
    public class CloudConfiguration :
        //Singleton<CloudConfiguration>,
        ICloudConfiguration
    {
        private const string CloudConfigurationAlreadyInitializedError = "CloudConfiguration object already initialized";
        private const string CloudConfigurationConfigSectionMissingError = "The cloudConfiguration configuration section is missing in the configuration file";
        private const string CloudConfigurationManagerConnectionStringSettingName = "StorageConnectionString";
        private const string CloudConfigurationSectionName = "cloudConfiguration";
        private const string CloudConfigurationUninitializedError = "CloudConfiguration uninitialized";

        private string _connectionString;
        private IRetryPolicy _globalRetryPolicy;
        private bool _initialized = false;
        private Func<bool> _isRunningInAzure;
        private Func<bool> _isRunningInAzureEmulator;
        private CloudStorageAccount _storageAccount;

        public CloudConfiguration()
        {
        }

        public string ConnectionString
        {
            get
            {
                Condition.Require<InvalidOperationException>(_initialized, CloudConfigurationUninitializedError);
                return _connectionString;
            }
        }

        public IRetryPolicy GlobalRetryPolicy
        {
            get
            {
                Condition.Require<InvalidOperationException>(_initialized, CloudConfigurationUninitializedError);

                if (_globalRetryPolicy == null)
                {
                    _globalRetryPolicy = new LinearRetry(new TimeSpan(0, 0, 0, 0, 500), 5);
                }

                return _globalRetryPolicy;
            }
        }

        public bool Initialized
        {
            get
            {
                return _initialized;
            }
        }

        public bool IsRunningInAzure
        {
            get
            {
                Condition.Require<InvalidOperationException>(_initialized, CloudConfigurationUninitializedError);
                return _isRunningInAzure();
            }
        }

        public bool IsRunningInEmulator
        {
            get
            {
                Condition.Require<InvalidOperationException>(_initialized, CloudConfigurationUninitializedError);
                return _isRunningInAzure() && _isRunningInAzureEmulator();
            }
        }

        public CloudStorageAccount StorageAccount
        {
            get
            {
                Condition.Require<InvalidOperationException>(_initialized, CloudConfigurationUninitializedError);

                if (_storageAccount == null)
                {
                    _storageAccount = CloudStorageAccount.Parse(ConnectionString);
                }

                return _storageAccount;
            }
        }

        public void Initialize(Func<bool> runningInAzure, Func<bool> runningInEmulator)
        {
            if (!_initialized)
            {
                Condition.ArgumentNotNull(runningInAzure, "runningInAzure");
                Condition.ArgumentNotNull(runningInEmulator, "runningInEmulator");

                _isRunningInAzure = runningInAzure;
                _isRunningInAzureEmulator = runningInEmulator;

                _initialized = true;

                // Determine the ConnectionString
                if (IsRunningInAzure)
                {
                    _connectionString = CloudConfigurationManager.GetSetting(CloudConfigurationManagerConnectionStringSettingName);
                }
                else
                {
                    var configSection = GetConfigurationSection<CloudConfigurationSection>(CloudConfigurationSectionName);
                    Condition.Require<ConfigurationException>(configSection != null, CloudConfigurationConfigSectionMissingError);
                    _connectionString = configSection.StorageConnectionString;
                }
            }
        }

        private T GetConfigurationSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }
}