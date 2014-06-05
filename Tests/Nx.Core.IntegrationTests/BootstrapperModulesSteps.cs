using System;
using TechTalk.SpecFlow;

namespace Nx.Core.IntegrationTests
{
    [Binding]
    public class BootstrapperModulesSteps
    {
        [Given(@"I have created (.*) module of type Nx\.Core\.Tests\.Bootstrappers\.TestModule")]
public void GivenIHaveCreatedModuleOfTypeNx_Core_Tests_Bootstrappers_TestModule(int p0)
{
    ScenarioContext.Current.Pending();
}

        [Given(@"I have injected all modules into the bootstrapper")]
public void GivenIHaveInjectedAllModulesIntoTheBootstrapper()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the number of modules present should be (.*)")]
public void ThenTheNumberOfModulesPresentShouldBe(int p0)
{
    ScenarioContext.Current.Pending();
}

        [Then(@"all modules should be loaded")]
public void ThenAllModulesShouldBeLoaded()
{
    ScenarioContext.Current.Pending();
}
    }
}
