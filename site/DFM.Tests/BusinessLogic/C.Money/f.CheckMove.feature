Feature: f. Check Move

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category

Scenario: 01. Mark a not checked move as checked
	Given I have a move with value 10 (Out)
		And the move is not checked
	When I try to mark it as checked
	Then I will receive no core error
		And the move will be checked

Scenario: 02. Remark a checked move as checked
	Given I have a move with value 10 (Out)
		And the move is checked
	When I try to mark it as checked
	Then I will receive this core error: MoveAlreadyChecked
		And the move will be checked
