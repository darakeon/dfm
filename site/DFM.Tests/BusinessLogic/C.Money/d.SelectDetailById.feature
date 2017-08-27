Feature: d. Get Detail

Background:
	Given I have an active user
	And I have two accounts
	And I have a category
	And I have a move with details

Scenario: 01. Try to get Detail with wrong ID (E)
	Given I pass an id of Detail that doesn't exist
	When I try to get the detail
	Then I will receive this core error: InvalidDetail
	And I will receive no detail

Scenario: 99. Get the Detail by ID (S)
	Given I pass valid Detail ID
	When I try to get the detail
	Then I will receive no core error
	And I will receive the detail
