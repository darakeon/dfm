Feature: Ed. Check move

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I enable move check

Scenario: Ed01. Mark a move Out as checked Out
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
	When I try to mark it as checked for account Out
	Then I will receive no core error
		And the move will be checked for account Out

Scenario: Ed02. Mark a move Out as checked In
	Given I have a move with value 10 (Out)
	When I try to mark it as checked for account In
	Then I will receive this core error: MoveCheckWrongNature
		And the move will not be checked for account In

Scenario: Ed03. Mark a move In as checked In
	Given I have a move with value 10 (In)
		And the move is not checked for account In
	When I try to mark it as checked for account In
	Then I will receive no core error
		And the move will be checked for account In

Scenario: Ed04. Mark a move In as checked Out
	Given I have a move with value 10 (In)
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveCheckWrongNature
		And the move will not be checked for account Out

Scenario: Ed05. Mark a move Transfer as checked In and Out
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account Out
		And I try to mark it as checked for account In
	Then I will receive no core error
		And the move will be checked for account Out
		And the move will be checked for account In

Scenario: Ed06. Mark a move Transfer as checked Out
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account Out
	Then I will receive no core error
		And the move will be checked for account Out
		And the move will not be checked for account In

Scenario: Ed07. Mark a move Transfer as checked In
	Given I have a move with value 10 (Transfer)
		And the move is not checked for account Out
		And the move is not checked for account In
	When I try to mark it as checked for account In
	Then I will receive no core error
		And the move will not be checked for account Out
		And the move will be checked for account In

Scenario: Ed08. Remark a checked move as checked
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveAlreadyChecked
		And the move will be checked for account Out

Scenario: Ed09. Mark a move as not checked with disabled setting
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
		And I disable move check
	When I try to mark it as checked for account Out
	Then I will receive this core error: MoveCheckDisabled
		And the move will not be checked for account Out

Scenario: Ed10. Mark another user's move as not checked
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
		But there is a bad person logged in
	When I try to mark it as checked for account Out
	Then I will receive this core error: InvalidMove
	Given test user login
	Then the move will not be checked for account Out

Scenario: Ed11. Mark a move as checked in closed account
	Given I have a move with value 10 (Out) at account Cf11
		And the move is not checked for account Out
		And I close the account Cf11
	When I try to mark it as checked for account Out
	Then I will receive no core error
		And the move will be checked for account Out

Scenario: Ed12. Not mark as checked if user is marked for deletion
	Given I have a move with value 10 (Out)
		But the user is marked for deletion
	When I try to mark it as checked for account Out
	Then I will receive this core error: UserDeleted

Scenario: Ed13. Not mark as checked if user requested wipe
	Given I have a move with value 10 (Out)
		But the user asked data wipe
	When I try to mark it as checked for account Out
	Then I will receive this core error: UserAskedWipe

Scenario: Ed14. Not mark as checked if not signed last contract
	Given I have a move with value 10 (Out)
		But there is a new contract
	When I try to mark it as checked for account Out
	Then I will receive this core error: NotSignedLastContract
