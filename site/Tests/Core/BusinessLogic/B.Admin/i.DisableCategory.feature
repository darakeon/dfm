Feature: Bi. Disable category

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have a category

Scenario: Bi01. Disable a Category that doesn't exist
	Given I pass a name of category that doesn't exist
	When I try to disable the category
	Then I will receive this core error: InvalidCategory

Scenario: Bi02. Disable a Category already disabled
	Given I give the enabled category Bi02
		And I already have disabled the category
	When I try to disable the category
	Then I will receive this core error: DisabledCategory

Scenario: Bi03. Disable a Category with info all right
	Given I give the enabled category Bi99
	When I try to disable the category
	Then I will receive no core error
		And the category will be disabled
