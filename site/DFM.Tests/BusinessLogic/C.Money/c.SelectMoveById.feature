Feature: Cc. Get Move

Background:
	Given I have an active user
		And the user have accepted the contract
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I have a move

Scenario: Cc01. Try to get Move with wrong ID
	Given I pass an id of Move that doesn't exist
	When I try to get the move
	Then I will receive this core error: InvalidMove
		And I will receive no move

Scenario: Cc02. Get the Move by ID
	Given I pass valid Move ID
	When I try to get the move
	Then I will receive no core error
		And I will receive the move
