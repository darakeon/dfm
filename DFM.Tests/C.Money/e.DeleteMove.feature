Feature: e. Delete of Moves

Scenario: 01. Try to delete Move with wrong ID (E)
	Given I pass an id of Move that doesn't exist
	When I try to delete the move
	Then I will receive this error: InvalidID
	And the move will not be deleted

Scenario: 99. Delete the Move by ID (S)
	Given I pass valid Detail ID
	When I try to delete the move
	Then I will receive no error
	And the move will be deleted
