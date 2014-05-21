using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
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

        [AfterScenario()]
        public void CleanUp()
        {
            if (_bootstrapper != null) _bootstrapper.Dispose();
            if (_kernel != null) _kernel.Dispose();
        }
    }
}