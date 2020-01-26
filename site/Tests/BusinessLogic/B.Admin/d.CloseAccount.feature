Feature: Bd. Close account

Background:
	Given I have a complete user logged in
		And I have an account
		And I disable Categories use

Scenario: Bd01. Close an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to close the account
	Then I will receive this core error: InvalidAccount

Scenario: Bd02. Close an Account already closed
	Given I give a url of the account BdE02 with moves
		And I already have closed the account
	When I try to close the account
	Then I will receive this core error: ClosedAccount

Scenario: Bd03. Close an Account that has no moves
	Given I give a url of the account BdE03 without moves
	When I try to close the account
	Then I will receive this core error: CantCloseEmptyAccount
		And the account will not be closed

Scenario: Bd04. Close an Account with info all right
	Given I give a url of the account BdS01 with moves
	When I try to close the account
	Then I will receive no core error
		And the account will be closed

Scenario: Bd05. Close an Account with schedule
	Given I give a url of the account BdS02 with moves
		And the account has a schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the schedule will be disabled

Scenario: Bd06. Close an Account with disabled schedule
	Given I give a url of the account BdS03 with moves
		And the account has a disabled schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
