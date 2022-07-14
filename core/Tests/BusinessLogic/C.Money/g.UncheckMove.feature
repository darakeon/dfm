Feature: Cg. Uncheck move

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I enable move check

Scenario: Cg01. Mark a checked move Out as not checked Out
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I try to mark it as not checked for account Out
	Then I will receive no core error
		And the move will not be checked for account Out

Scenario: Cg02. Mark a checked move Out as not checked In
	Given I have a move with value 10 (Out)
	When I try to mark it as not checked for account In
	Then I will receive this core error: MoveCheckWrongNature

Scenario: Cg03. Mark a checked move In as not checked In
	Given I have a move with value 10 (In)
		And the move is checked for account In
	When I try to mark it as not checked for account In
	Then I will receive no core error
		And the move will not be checked for account In

Scenario: Cg04. Mark a checked move In as not checked Out
	Given I have a move with value 10 (In)
	When I try to mark it as not checked for account Out
	Then I will receive this core error: MoveCheckWrongNature

Scenario: Cg05. Mark a checked move Transfer as not checked In and Out
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I try to mark it as not checked for account Out
		And I try to mark it as not checked for account In
	Then I will receive no core error
		And the move will not be checked for account Out
		And the move will not be checked for account In

Scenario: Cg06. Mark a checked move Transfer as not checked Out
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I try to mark it as not checked for account Out
	Then I will receive no core error
		And the move will not be checked for account Out
		And the move will be checked for account In

Scenario: Cg07. Mark a checked move Transfer as not checked In
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I try to mark it as not checked for account In
	Then I will receive no core error
		And the move will be checked for account Out
		And the move will not be checked for account In

Scenario: Cg08. Remark a not checked move as not checked
	Given I have a move with value 10 (Out)
		And the move is not checked for account Out
	When I try to mark it as not checked for account Out
	Then I will receive this core error: MoveAlreadyUnchecked
		And the move will not be checked for account Out

Scenario: Cg09. Mark a move as not checked with disabled setting
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		And I disable move check
	When I try to mark it as not checked for account Out
	Then I will receive this core error: MoveCheckDisabled
		And the move will be checked for account Out

Scenario: Cg10. Mark another user's move as not checked
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		But there is a bad person logged in
	When I try to mark it as not checked for account Out
	Then I will receive this core error: InvalidMove
	Given test user login
	Then the move will be checked for account Out

Scenario: Cg11. Mark a move as checked in closed account
	Given I have a move with value 10 (Out) at account Cg11
		And the move is checked for account Out
		And I close the account Cg11
	When I try to mark it as not checked for account Out
	Then I will receive no core error
		And the move will not be checked for account Out

Scenario: Cg12. Not mark as not checked if user is marked for deletion
	Given I have a move with value 10 (Out)
		But the user is marked for deletion
	When I try to mark it as not checked for account Out
	Then I will receive this core error: UserDeleted

Scenario: Cg13. Not mark as not checked if user requested wipe
	Given I have a move with value 10 (Out)
		But the user asked data wipe
	When I try to mark it as not checked for account Out
	Then I will receive this core error: UserAskedWipe
