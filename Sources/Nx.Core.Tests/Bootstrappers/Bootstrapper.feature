Feature: Bootstrapper

@Bootstrapper
Scenario: Run the bootstrapper
	Given I have created the Bootstrapper
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the ILogger shall be registered

@Bootstrapper @Bootstrapper_extension @Object_instance
Scenario: Extend the bootstrapper with an object instance
	Given I have created the Bootstrapper
	And  I extend the Bootstrapper with an object instance
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered

@Bootstrapper @Bootstrapper_extension @Generic_type
Scenario: Extend the bootstrapper with a generic type
	Given I have created the Bootstrapper
	And  I extend the Bootstrapper with a generic type
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered