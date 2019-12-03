Feature: Cf. Check move

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I enable move check

Scenario: Cf01. Mark a not checked move as checked
	Given I have a move with value 10 (Out)
		And the move is not checked
	When I try to mark it as checked
	Then I will receive no core error
		And the move will be checked

Scenario: Cf02. Remark a checked move as checked
	Given I have a move with value 10 (Out)
		And the move is checked
	When I try to mark it as checked
	Then I will receive this core error: MoveAlreadyChecked
		And the move will be checked

Scenario: Cf03. Mark a move as not checked with disabled config
	Given I have a move with value 10 (Out)
		And the move is not checked
		And I disable move check
	When I try to mark it as checked
	Then I will receive this core error: MoveCheckDisabled
		And the move will not be checked

Scenario: Cf04. Mark another user's move as not checked
	Given I have a move with value 10 (Out)
		And the move is not checked
		But there is a bad person logged in
	When I try to mark it as checked
	Then I will receive this core error: InvalidMove
	Given the right user login again
	Then the move will not be checked
