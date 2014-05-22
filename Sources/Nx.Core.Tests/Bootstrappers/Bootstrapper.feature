Feature: Bootstrapper

@Bootstrapper
Scenario: Run the bootstrapper
	Given I have created the Bootstrapper
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the ILogger shall be registered

@Bootstrapper_extension_object_instance
Scenario: Extend the bootstrapper with an object instance
	Given I have created the Bootstrapper
	Given  I extend the Bootstrapper with an object instance
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered

@Bootstrapper_extension_generic_typea
Scenario: Extend the bootstrapper with a generic type
	Given I have created the Bootstrapper
	Given  I extend the Bootstrapper with a generic type
	When  I have run the Bootstrapper
	Then the kernel shall not be null
	And the TestType shall be registered