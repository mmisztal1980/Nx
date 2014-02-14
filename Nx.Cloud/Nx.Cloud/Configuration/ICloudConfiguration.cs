using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;

namespace Nx.Cloud.Configuration
{
    public interface ICloudConfiguration
    {
        /// <summary>
        /// Initialization function. Must be called once.
        /// </summary>
        /// <param name="runningInAzure">Delegate returning a boolean determining if the application is running in Azure RoleEnvironment</param>
        /// <param name="runningInEmulator">Delegate returning a boolean determining if the application is running in the Azure RoleEnvironment Emulator</param>
        void Initialize(Func<bool> runningInAzure, Func<bool> runningInEmulator);

        /// <summary>
        /// Boolean flag determining if the CloudConfiguration has been initialized
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Boolean flag determining if the CloudConfiguration is running in Azure RoleEnvironment
        /// </summary>
        bool IsRunningInAzure { get; }

        /// <summary>
        /// Boolean flag determining if the CloudConfiguration is running in Emulated Azure RoleEnvironment
        /// </summary>
        bool IsRunningInEmulator { get; }

        /// <summary>
        /// The Storage connection string
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// The Cloud Storage Account
        /// </summary>
        CloudStorageAccount StorageAccount { get; }

        /// <summary>
        /// The Global Retry Policy to be used in all storage mechanisms
        /// </summary>
        IRetryPolicy GlobalRetryPolicy { get; }
    }
}
