Feature: Cj. Get category

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have a category

Scenario: Cj01. Try to get Category with wrong Name
	Given I pass a name of category that doesn't exist
	When I try to get the category by its name
	Then I will receive this core error: InvalidCategory
		And I will receive no category

Scenario: Cj02. Get the Category by Name
	Given I pass a valid category name
	When I try to get the category by its name
	Then I will receive no core error
		And I will receive the category

Scenario: Cj03. Not get category if user is marked for deletion
	Given I pass a valid category name
		But the user is marked for deletion
	When I try to get the category by its name
	Then I will receive this core error: UserDeleted

Scenario: Cj04. Not get category if user requested wipe
	Given I pass a valid category name
		But the user asked data wipe
	When I try to get the category by its name
	Then I will receive this core error: UserAskedWipe

Scenario: Cj05. Not get Category without signing contract
	Given I pass a valid category name
		But there is a new contract
	When I try to get the category by its name
	Then I will receive this core error: NotSignedLastContract
