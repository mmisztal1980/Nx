using System.Diagnostics;

namespace Nx.Cloud.Tests
{
    public abstract class CloudTestFixtureBase : TestFixtureBase
    {
        protected CloudTestFixtureBase()
            : base()
        {
            if (!StorageEmulatorIsRunning)
            {
                //Process.Start(@"C:\Program Files\Microsoft SDKs\Windows Azure\Emulator\csrun", "/devstore").WaitForExit();
            }
        }

        protected bool StorageEmulatorIsRunning
        {
            get
            {
                var count = Process.GetProcessesByName("DSServiceLDB").Length;
                return count > 0;
            }
        }
    }
}