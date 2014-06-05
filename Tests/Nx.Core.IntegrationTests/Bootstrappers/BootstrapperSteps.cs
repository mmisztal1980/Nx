using System;
using TechTalk.SpecFlow;

namespace Nx.Core.IntegrationTests.Bootstrappers
{
    [Binding]
    public class BootstrapperSteps
    {
        [Given(@"I have created the Bootstrapper")]
public void GivenIHaveCreatedTheBootstrapper()
{
    ScenarioContext.Current.Pending();
}

        [Given(@"I extend the Bootstrapper with an object instance")]
public void GivenIExtendTheBootstrapperWithAnObjectInstance()
{
    ScenarioContext.Current.Pending();
}

        [Given(@"I extend the Bootstrapper with a generic type")]
public void GivenIExtendTheBootstrapperWithAGenericType()
{
    ScenarioContext.Current.Pending();
}

        [When(@"I have run the Bootstrapper")]
public void WhenIHaveRunTheBootstrapper()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the kernel shall not be null")]
public void ThenTheKernelShallNotBeNull()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the ILogger shall be registered")]
public void ThenTheILoggerShallBeRegistered()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the TestType shall be registered")]
public void ThenTheTestTypeShallBeRegistered()
{
    ScenarioContext.Current.Pending();
}
    }
}
