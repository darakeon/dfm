Feature: Cf. Check move

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I enable move check

Scenario: Cf01. Mark a move Out as checked Out
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
	When I try to mark it as checked for account Out
	Then I will receive no core error
		And the move will be checked for account Out

Scenario: Cf02. Mark a move Out as checked In
	Given I have a move with value 10 (Out)
	When I try to mark it as checked for account In
	Then I will receive this core error: MoveCheckWrongNature
		And the move will not be checked for account In

Scenario: Cf03. Mark a move In as checked In
	Given I have a move with value 10 (In)
		And the move is not checked for account In
	When I try to mark it as checked for account In
	Then I will receive no core error
		And the move will be checked for account In

Scenario: Cf04. Mark a move In as checked Out
	Given I have a move with value 10 (In)
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveCheckWrongNature
		And the move will not be checked for account Out

Scenario: Cf05. Mark a move Transfer as checked In and Out
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account Out
		And I try to mark it as checked for account In
	Then I will receive no core error
		And the move will be checked for account Out
		And the move will be checked for account In

Scenario: Cf06. Mark a move Transfer as checked Out
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account Out
	Then I will receive no core error
		And the move will be checked for account Out
		And the move will not be checked for account In

Scenario: Cf07. Mark a move Transfer as checked In
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account In
	Then I will receive no core error
		And the move will not be checked for account Out
		And the move will be checked for account In

Scenario: Cf08. Remark a checked move as checked
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveAlreadyChecked
		And the move will be checked for account Out

Scenario: Cf09. Mark a move as not checked with disabled config
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
		And I disable move check
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveCheckDisabled
		And the move will not be checked for account Out

Scenario: Cf10. Mark another user's move as not checked
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
		But there is a bad person logged in
	When I try to mark it as checked for account Out
	Then I will receive this core error: InvalidMove
	Given the right user login again
	Then the move will not be checked for account Out
