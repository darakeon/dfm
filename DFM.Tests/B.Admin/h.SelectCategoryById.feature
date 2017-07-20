Feature: h. Get Category

Background:
	Given I have an user
	And I have a category

Scenario: 01. Try to get Category with wrong ID (E)
	Given I pass an id the doesn't exist
	When I try to get the category
	Then I will receive this error
		| Error     |
		| InvalidID |
	And I will receive no category

Scenario: 99. Get the Category by ID (S)
	Given I pass valid ID
	When I try to get the category
	Then I will receive no error
	And I will receive the category