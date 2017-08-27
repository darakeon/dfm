Feature: c. Get Move

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category
	And I have a move

Scenario: 01. Try to get Move with wrong ID (E)
	Given I pass an id of Move that doesn't exist
	When I try to get the move
	Then I will receive this core error: InvalidMove
	And I will receive no move

Scenario: 99. Get the Move by ID (S)
	Given I pass valid Move ID
	When I try to get the move
	Then I will receive no core error
	And I will receive the move
