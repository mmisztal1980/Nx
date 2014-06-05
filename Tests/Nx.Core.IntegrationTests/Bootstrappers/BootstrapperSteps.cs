using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Core.Tests.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;
using Nx.Modules;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Nx.Core.IntegrationTests.Bootstrappers
{
    [Binding]
    public class BootstrapperSteps
    {
        [Given(@"I have created the Bootstrapper")]
        public void GivenIHaveCreatedTheBootstrapper()
        {
            ScenarioContext.Current[BootstrapperTests.BootstrapperKey] = new Bootstrapper();
        }

        [When(@"I have run the Bootstrapper")]
        public void WhenIHaveRunTheBootstrapper()
        {
            var bootstrapper = ScenarioContext.Current[BootstrapperTests.BootstrapperKey] as BootstrapperBase;
            Assert.IsNotNull(bootstrapper);
            ScenarioContext.Current[BootstrapperTests.KernelKey] = bootstrapper.Run();
        }

        [Then(@"the ILogger shall be registered")]
        public void ThenTheILoggerShallBeRegistered()
        {
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
            Assert.IsTrue(kernel.IsRegistered<ILogger>());
        }

        [Then("the kernel shall not be null")]
        public void ThenTheKernelShallNotBeNull()
        {
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
        }

        [Given(@"I extend the Bootstrapper with an object instance")]
        public void WhenIExtendTheBootstrapperFluently()
        {
            var bootstrapper = ScenarioContext.Current[BootstrapperTests.BootstrapperKey] as BootstrapperBase;
            Assert.IsNotNull(bootstrapper);
            bootstrapper.ExtendBy(new TestExtension());
        }

        [Then(@"the TestType shall be registered")]
        public void ThenTheTestTypeShallBeRegistered()
        {
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
            Assert.IsTrue(kernel.IsRegistered<TestType>());
        }

        [Given(@"I extend the Bootstrapper with a generic type")]
        public void WhenIExtendTheBootstrapperGenerically()
        {
            var bootstrapper = ScenarioContext.Current[BootstrapperTests.BootstrapperKey] as BootstrapperBase;
            Assert.IsNotNull(bootstrapper);
            bootstrapper.ExtendBy<TestExtension>();
        }

        [AfterScenario()]
        public void CleanUp()
        {
            var bootstrapper = ScenarioContext.Current[BootstrapperTests.BootstrapperKey] as BootstrapperBase;
            if (bootstrapper != null)
            {
                bootstrapper.Dispose();
                ScenarioContext.Current.Remove(BootstrapperTests.BootstrapperKey);
            }

            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            if (kernel != null)
            {
                kernel.Dispose();
                ScenarioContext.Current.Remove(BootstrapperTests.KernelKey);
            }

            if (ScenarioContext.Current.ContainsKey(BootstrapperTests.ModulesKey))
            {
                var modules = ScenarioContext.Current[BootstrapperTests.ModulesKey] as List<Module>;
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        module.Dispose();
                    }

                    modules.Clear();

                    ScenarioContext.Current.Remove(BootstrapperTests.ModulesKey);
                }
            }
        }
    }
}