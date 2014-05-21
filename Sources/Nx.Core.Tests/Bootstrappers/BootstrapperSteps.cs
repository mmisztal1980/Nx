using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;
using TechTalk.SpecFlow;

namespace Nx.Core.Tests.Bootstrappers
{
    [Binding]
    public class BootstrapperSteps
    {
        private BootstrapperBase _bootstrapper;
        private IKernel _kernel;

        [Given(@"I have created the Bootstrapper")]
        public void GivenIHaveCreatedTheBootstrapper()
        {
            _bootstrapper = new Bootstrapper();
        }

        [When(@"I have run the Bootstrapper")]
        public void WhenIHaveRunTheBootstrapper()
        {
            Assert.IsNotNull(_bootstrapper);
            _kernel = _bootstrapper.Run();
        }

        [Then(@"the kernel shall not be null")]
        public void ThenTheKernelShallNotBeNull()
        {
            Assert.IsNotNull(_kernel);
        }

        [Then(@"the ILogger shall be registered")]
        public void ThenTheILoggerShallBeRegistered()
        {
            Assert.IsTrue(_kernel.IsRegistered<ILogger>());
        }

        [Given(@"I extend the Bootstrapper fluently")]
        public void WhenIExtendTheBootstrapperFluently()
        {
            _bootstrapper.ExtendBy(new TestExtension());
        }

        [Then(@"the TestType shall be registered")]
        public void ThenTheTestTypeShallBeRegistered()
        {
            Assert.IsTrue(_kernel.IsRegistered<TestType>());
        }

        [Given(@"I extend the Bootstrapper generically")]
        public void WhenIExtendTheBootstrapperGenerically()
        {
            _bootstrapper.ExtendBy<TestExtension>();
        }

        [AfterScenario()]
        public void CleanUp()
        {
            if (_bootstrapper != null) _bootstrapper.Dispose();
            if (_kernel != null) _kernel.Dispose();
        }
    }
}