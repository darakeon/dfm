Feature: d. Close Account

Background:
	Given I have an active user
	And I have an account

Scenario: E01. Close an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to close the account
	Then I will receive this core error: InvalidAccount

Scenario: E02. Close an Account already closed
	Given I have a category
	And I give a url of the account BdE02 with moves
	And I already have closed the account
	When I try to close the account
	Then I will receive this core error: ClosedAccount

Scenario: E03. Close an Account that has no moves
	Given I give a url of the account BdE03 without moves
	When I try to close the account
	Then I will receive this core error: CantCloseEmptyAccount
	And the account will not be closed


Scenario: S01. Close an Account with info all right
	Given I have a category
	And I give a url of the account BdS01 with moves
	When I try to close the account
	Then I will receive no core error
	And the account will be closed

Scenario: S02. Close an Account with schedule
	Given I have a category
	And I give a url of the account BdS02 with moves
	And the account has a schedule
	When I try to close the account
	Then I will receive no core error
	And the account will be closed
	And the schedule will be disabled

Scenario: S03. Close an Account with disabled schedule
	Given I have a category
	And I give a url of the account BdS03 with moves
	And the account has a disabled schedule
	When I try to close the account
	Then I will receive no core error
	And the account will be closed