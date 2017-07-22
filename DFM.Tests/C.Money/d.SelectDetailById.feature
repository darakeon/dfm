Feature: d. Get Detail

Background:
	Given I have an user
	And I have a move
	And I have a detail

Scenario: 01. Try to get Detail with wrong ID (E)
	Given I pass an id od Detail that doesn't exist
	When I try to get the detail
	Then I will receive this error
		| Error     |
		| InvalidID |
	And I will receive no detail

Scenario: 99. Get the Detail by ID (S)
	Given I pass valid Detail ID
	When I try to get the detail
	Then I will receive no error
	And I will receive the detail
