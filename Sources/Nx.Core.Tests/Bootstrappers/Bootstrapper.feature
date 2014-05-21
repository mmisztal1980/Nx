Feature: Bootstrapper

@Bootstrapper
Scenario: Run the bootstrapper
	Given I have created the Bootstrapper
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the ILogger shall be registered

Scenario: Fluently extend the bootstrapper
	Given I have created the Bootstrapper
	Given  I extend the Bootstrapper fluently
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered

Scenario: Generically extend the bootstrapper
	Given I have created the Bootstrapper
	Given  I extend the Bootstrapper generically
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered