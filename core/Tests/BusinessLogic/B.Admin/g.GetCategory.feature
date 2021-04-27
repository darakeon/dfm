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
