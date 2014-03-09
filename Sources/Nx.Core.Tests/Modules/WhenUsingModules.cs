using Ninject.Modules;
using NUnit.Framework;
using Nx.Core.Tests.Bootstrappers;
using Nx.Modules;
using System;

namespace Nx.Core.Tests.Modules
{
    [TestFixture]
    public class WhenUsingModules
    {
        [Test]
        public void ModuleShouldInitialize()
        {
            var modules = new INinjectModule[1]
                {
                    new TestModule()
                };

            using (var bootstrapper = new Bootstrapper(modules))
            using (var kernel = bootstrapper.Run())
            {
                foreach (var module in modules)
                {
                    var testModule = module as TestModule;
                    Assert.IsTrue(testModule != null && testModule.IsLoaded);
                    Assert.IsTrue(testModule.ModuleLogger != null);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void ModuleShouldNotInitializeMoreThenOnce()
        {
            var modules = new INinjectModule[3]
                {
                    new TestModule(),
                    new TestModule(),
                    new TestModule()
                };

            using (var bootstrapper = new Bootstrapper(modules))
            using (var kernel = bootstrapper.Run())
            {
                foreach (var module in modules)
                {
                    var testModule = module as TestModule;
                    Assert.IsTrue(testModule != null && testModule.IsLoaded);
                }
            }
        }

        private class TestModule : Module
        {
            public TestModule()
            {
                IsLoaded = false;
            }

            public bool IsLoaded { get; private set; }

            public override string Name
            {
                get { return "TestModule"; }
            }

            public override void OnDisposing()
            {
            }

            public override void OnLoading()
            {
                IsLoaded = true;
            }
        }
    }
}