namespace Nx.Cloud
{
    //public static class Environment
    //{
    //    private static string StorageSettingName = "Storage";

    //    #region Settings
    //    public static void InitializeStorageSettings()
    //    {
    //        CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
    //        {
    //            configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
    //        });
    //    }

    //    /// <summary>
    //    /// Requires a setting "Storage" for Cloud and Emulator configurations specifying the connection to the Azure storage
    //    /// </summary>
    //    /// <returns></returns>
    //    public static string GetStorageConnectionString()
    //    {
    //        return GetStorageConnectionString(false);
    //    }

    //    /// <summary>
    //    /// Requires a setting "Storage" for Cloud and Emulator configurations specifying the connection to the Azure storage
    //    /// </summary>
    //    /// <returns></returns>
    //    public static string GetStorageConnectionString(bool notRunningInAzure)
    //    {
    //        if (Environment.IsRunningInAzure && !notRunningInAzure)
    //        {
    //            return RoleEnvironment.GetConfigurationSettingValue(StorageSettingName);
    //        }
    //        else
    //        {
    //            return ConfigurationManager.ConnectionStrings[StorageSettingName].ConnectionString;
    //        }
    //    }

    //    public static CloudStorageAccount GetStorageAccount()
    //    {
    //        return CloudStorageAccount.FromConfigurationSetting(Environment.StorageSettingName);
    //    }
    //    #endregion

    //    #region Diagnostics
    //    public static void InitializeDiagnosticsMonitoring(string diagnosticsConnectionStringSettingName)
    //    {
    //        DiagnosticMonitor.Start(diagnosticsConnectionStringSettingName);
    //    }

    //    public static void InitializeDiagnosticsMonitoring(string diagnosticsConnectionStringSettingName, DiagnosticMonitorConfiguration configuration)
    //    {
    //        DiagnosticMonitor.Start(diagnosticsConnectionStringSettingName, configuration);
    //    }
    //    #endregion

    //    /// <summary>
    //    /// Indicates whether this code is being executed in a Azure role.
    //    /// </summary>
    //    public static bool IsRunningInAzure
    //    {
    //        get
    //        {
    //            return RoleEnvironment.IsAvailable;
    //        }
    //    }

    //    /// <summary>
    //    /// Indicates whether this code is being executed in a Azure Compute Emulator.
    //    /// </summary>
    //    public static bool IsEmulated
    //    {
    //        get
    //        {
    //            return IsRunningInAzure && RoleEnvironment.IsEmulated;
    //        }
    //    }
    //}
}
