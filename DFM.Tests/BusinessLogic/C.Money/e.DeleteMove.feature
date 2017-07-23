Feature: e. Delete of Moves

Background:
	Given I have an user
	And I have two accounts
	And I have a category
	And I have a move with value 10 (Out)

Scenario: 01. Try to delete Move with wrong ID (E)
	Given I pass an id of Move that doesn't exist
	When I try to delete the move
	Then I will receive this core error: InvalidMove
	And the accountOut value will not change

Scenario: 99. Delete the Move by ID (S)
	Given I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
	And the move will be deleted
	And the accountOut value will change in 10
