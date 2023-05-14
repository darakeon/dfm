Feature: Bg. Get category

Background:
	Given test user login
		And I enable Categories use
		And I have a category

Scenario: Bg01. Try to get Category with wrong Name
	Given I pass a name of category that doesn't exist
	When I try to get the category by its name
	Then I will receive this core error: InvalidCategory
		And I will receive no category

Scenario: Bg02. Get the Category by Name
	Given I pass a valid category name
	When I try to get the category by its name
	Then I will receive no core error
		And I will receive the category

Scenario: Bg03. Not get category if user is marked for deletion
	Given I pass a valid category name
		But the user is marked for deletion
	When I try to get the category by its name
	Then I will receive this core error: UserDeleted

Scenario: Bg04. Not get category if user requested wipe
	Given I pass a valid category name
		But the user asked data wipe
	When I try to get the category by its name
	Then I will receive this core error: UserAskedWipe

Scenario: Bg05. Not get Category without signing contract
	Given I pass a valid category name
		But there is a new contract
	When I try to get the category by its name
	Then I will receive this core error: NotSignedLastContract
