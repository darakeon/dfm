Feature: g. Get Category by Name

Background:
	Given I have an user
	And I have a category

Scenario: 01. Try to get Category with wrong Name (E)
	Given I pass a name of category that doesn't exist
	When I try to get the category by its Name
	Then I will receive this error: InvalidCategory
	And I will receive no category

Scenario: 99. Get the Category by Name (S)
	Given I pass valid category Name
	When I try to get the category by its Name
	Then I will receive no error
	And I will receive the category