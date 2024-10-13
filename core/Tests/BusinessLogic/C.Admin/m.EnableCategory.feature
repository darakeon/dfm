﻿Feature: Cm. Enable category

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have a category

Scenario: Cm01. Enable a Category that doesn't exist
	Given I pass a name of category that doesn't exist
	When I try to enable the category
	Then I will receive this core error: InvalidCategory

Scenario: Cm02. Enable a Category already enabled
	Given I give the disabled category Bj02
		And I already have enabled the category
	When I try to enable the category
	Then I will receive this core error: EnabledCategory

Scenario: Cm03. Enable a Category with info all right
	Given I give the disabled category Bj03
	When I try to enable the category
	Then I will receive no core error
		And the category will be enabled

Scenario: Cm04. Not enable Category if user is marked for deletion
	Given I give the disabled category Bj04
		But the user is marked for deletion
	When I try to enable the category
	Then I will receive this core error: UserDeleted

Scenario: Cm05. Not enable Category if user requested wipe
	Given I give the disabled category Bj05
		But the user asked data wipe
	When I try to enable the category
	Then I will receive this core error: UserAskedWipe

Scenario: Cm06. Enable Category without signing contract
	Given I give the disabled category Bj06
		But there is a new contract
	When I try to enable the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Cm07. Not reopen an Category above limits
	Given these limits in user plan
			| CategoryEnabled |
			| 4               |
		And I have these categories
			| Name       |
			| Category 1 |
			| Category 2 |
			| Category 3 |
		And I disable the category Category 1
		And I have these categories
			| Name       |
			| Category 4 |
	When I try to enable the category
	Then I will receive this core error: PlanLimitCategoryEnabledAchieved
		And the category will not be enabled
