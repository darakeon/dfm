Feature: g. Uncheck Move 

Background:
	Given I have an active user
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I enable move check

Scenario: 01. Mark a checked move as not checked
	Given I have a move with value 10 (Out)
		And the move is checked
	When I try to mark it as not checked
	Then I will receive no core error
		And the move will not be checked

Scenario: 02. Remark a not checked move as not checked
	Given I have a move with value 10 (Out)
		And the move is not checked
	When I try to mark it as not checked
	Then I will receive this core error: MoveAlreadyUnchecked
		And the move will not be checked

Scenario: 03. Mark a move as not checked with disabled config
	Given I have a move with value 10 (Out)
		And the move is checked
		And I disable move check
	When I try to mark it as not checked
	Then I will receive this core error: MoveCheckDisabled
		And the move will be checked
