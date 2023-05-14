Feature: Bj. Enable category

Background:
	Given test user login
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
	Given I give the disabled category Bj03
	When I try to enable the category
	Then I will receive no core error
		And the category will be enabled

Scenario: Bj04. Not enable Category if user is marked for deletion
	Given I give the disabled category Bj04
		But the user is marked for deletion
	When I try to enable the category
	Then I will receive this core error: UserDeleted

Scenario: Bj05. Not enable Category if user requested wipe
	Given I give the disabled category Bj05
		But the user asked data wipe
	When I try to enable the category
	Then I will receive this core error: UserAskedWipe

Scenario: Bj06. Enable Category without signing contract
	Given I give the disabled category Bj06
		But there is a new contract
	When I try to enable the category
	Then I will receive this core error: NotSignedLastContract
