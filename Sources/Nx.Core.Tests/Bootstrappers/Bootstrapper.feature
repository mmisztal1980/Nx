Feature: Bootstrapper

@Bootstrapper
Scenario: Run the bootstrapper
	Given I have created the Bootstrapper
	When  I have run the Bootstrapper
	Then the kernel shall not be null