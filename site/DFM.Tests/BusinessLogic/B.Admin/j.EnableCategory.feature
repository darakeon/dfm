Feature: Bj. Enable Category

Background:
	Given I have an active user who have accepted the contract
		And I enable Categories use
		And I have a category

Scenario: Bj01. Enable a Category that doesn't exist
	Given I pass a name of category that doesn't exist
	When I try to enable the category
	Then I will receive this core error: InvalidCategory

Scenario: Bj02. Enable a Category already enabled
	Given I give the disabled category Bj02
		And I already have enabled the category
	When I try to enable the category
	Then I will receive this core error: EnabledCategory

Scenario: Bj03. Enable a Category with info all right
	Given I give the disabled category Bj99
	When I try to enable the category
	Then I will receive no core error
		And the category will be enabled
