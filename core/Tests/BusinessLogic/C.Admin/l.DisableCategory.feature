Feature: Cl. Disable category

Background:
	Given test user login
		And I enable Categories use
		And I have a category

Scenario: Cl01. Disable a Category that doesn't exist
	Given I pass a name of category that doesn't exist
	When I try to disable the category
	Then I will receive this core error: InvalidCategory

Scenario: Cl02. Disable a Category already disabled
	Given I give the enabled category Bi02
		And I already have disabled the category
	When I try to disable the category
	Then I will receive this core error: DisabledCategory

Scenario: Cl03. Disable a Category with info all right
	Given I give the enabled category Bi03
	When I try to disable the category
	Then I will receive no core error
		And the category will be disabled

Scenario: Cl04. Not disable Category if user is marked for deletion
	Given I give the enabled category Bi04
		But the user is marked for deletion
	When I try to disable the category
	Then I will receive this core error: UserDeleted

Scenario: Cl05. Not disable Category if user requested wipe
	Given I give the enabled category Bi05
		But the user asked data wipe
	When I try to disable the category
	Then I will receive this core error: UserAskedWipe

Scenario: Cl06. Not disable Category without signing contract
	Given I give the enabled category Bi06
		But there is a new contract
	When I try to disable the category
	Then I will receive this core error: NotSignedLastContract
