Feature: BootstrapperModules

@Bootstrapper @Module_loading
Scenario: Run the bootstrapper and inject single module
	Given I have created 1 module of type Nx.Core.Tests.Bootstrappers.TestModule
	And I have injected all modules into the bootstrapper
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the number of modules present should be 1
	And all modules should be loaded

