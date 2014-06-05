using Ninject;
using NUnit.Framework;
using Nx.Core.Tests.Bootstrappers;
using Nx.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Nx.Core.IntegrationTests.Bootstrappers
{
    [Binding]
    public class BootstrapperModulesSteps
    {
        private readonly List<Module> _modules = new List<Module>();

        [Given(@"I have created (.*) module of type (.*)")]
        public void GivenIHaveCreatedModuleOfTypeTypeofTestModule(int p0, string p1)
        {
            Assert.IsTrue(p0 > 0);
            Assert.IsNotNullOrEmpty(p1);

            var modules = new List<Module>();
            for (int i = 0; i < p0; i++)
            {
                modules.Add((Module)Activator.CreateInstance(Type.GetType(p1)));
            }

            ScenarioContext.Current[BootstrapperTests.ModulesKey] = modules;
        }

        [Given(@"I have injected all modules into the bootstrapper")]
        public void GivenIHaveInjectedAllModulesIntoTheBootstrapper()
        {
            var modules = ScenarioContext.Current[BootstrapperTests.ModulesKey] as List<Module>;
            Assert.IsNotNull(modules);
            ScenarioContext.Current[BootstrapperTests.BootstrapperKey] = new Bootstrapper(modules.ToArray());
        }

        [Then(@"all modules should be loaded")]
        public void ThenAllModulesShouldBeLoaded()
        {
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
            var modules = kernel.GetModules();
            foreach (var module in modules)
            {
                var loadableModule = module as ILoadable;
                if (loadableModule != null)
                {
                    Assert.IsTrue(loadableModule.IsLoaded);
                }
            }
        }

        [Then(@"the number of modules present should be (.*)")]
        public void TheNumberOfModulesPresentShouldBe(int p0)
        {
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
            var modules = kernel.GetModules();
            Assert.AreEqual(modules.Count(), p0);
        }
    }
}