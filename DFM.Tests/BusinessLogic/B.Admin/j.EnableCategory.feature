Feature: j. Enable Category

Background:
	Given I have an active user
	And I have a category

Scenario: 01. Enable a Category that doesn't exist (E)
	Given I pass a name of category that doesn't exist
	When I try to enable the category
	Then I will receive this core error: InvalidCategory

Scenario: 02. Enable a Category already enabled (E)
	Given I give an id of disabled category Bj02
	And I already have enabled the category
	When I try to enable the category
	Then I will receive this core error: EnabledCategory

Scenario: 99. Enable a Category with info all right (S)
	Given I give an id of disabled category Bj99
	When I try to enable the category
	Then I will receive no core error
	And the category will be enabled