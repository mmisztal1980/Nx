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
                Console.WriteLine("Added module {0}/{1} {2}", i, p0, modules.Last().GetHashCode());
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
            Console.WriteLine("Expected number of modules : {0}", p0);
            var kernel = ScenarioContext.Current[BootstrapperTests.KernelKey] as IKernel;
            Assert.IsNotNull(kernel);
            var modules = kernel.GetModules();

            foreach (var module in modules)
            {
                Console.WriteLine("Module found : {0}, {1}", module.GetHashCode(), module.GetType().ToString());
            }

            Assert.AreEqual(p0, modules.Count());
        }

        [AfterScenario]
        public void CleanUp()
        {
            if (ScenarioContext.Current.ContainsKey(BootstrapperTests.ModulesKey))
            {
                var modules = ScenarioContext.Current[BootstrapperTests.ModulesKey] as List<Module>;
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        Console.WriteLine("Disposing module {0}", module.GetHashCode());
                        module.Dispose();
                    }

                    modules.Clear();

                    ScenarioContext.Current.Remove(BootstrapperTests.ModulesKey);
                }
            }
        }
    }
}