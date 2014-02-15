using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Cloud.Tests;
using Nx.Extensions;
using Nx.Logging;

/// <summary>
/// The Assembly setup is intentionally in global namespace. This will be run prior to ALL test fixtures in the assembly.
/// http://www.ncrunch.net/documentation/considerations-and-constraints_test-atomicity
/// </summary>
[SetUpFixture]
// ReSharper disable CheckNamespace
public class AssemblySetup
// ReSharper restore CheckNamespace
{
    public static bool AssemblyInitialized = false;
    public static int InitializationCount = 0;
    private static readonly object Lock = new object();

    private ILogger _logger;

    public static IKernel Kernel { get; private set; }

    [SetUp]
    public void SetUp()
    {
        if (!AssemblyInitialized)
        {
            lock (Lock)
            {
                if (!AssemblyInitialized)
                {
                    using (IBootstrapper bootstrapper = new Bootstrapper()
                            .ExtendBy<NxLoggingExtension>())
                    {
                        Kernel = bootstrapper.Run();
                        InitializationCount++;
                        _logger = Kernel.Get<ILogFactory>().CreateLogger("AssemblySetup");
                        _logger.Info("Test assembly has been set up");
                    }

                    AssemblyInitialized = true;
                }
            }
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (AssemblyInitialized)
        {
            lock (Lock)
            {
                if (AssemblyInitialized)
                {
                    if (Kernel != null)
                    {
                        _logger.Info("Test assembly is being torn down");
                        Kernel.Dispose();
                        _logger.Dispose();
                    }

                    AssemblyInitialized = false;
                }
            }
        }
    }
}