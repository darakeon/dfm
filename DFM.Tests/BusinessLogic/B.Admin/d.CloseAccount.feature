Feature: d. Close Account

Background:
	Given I have an active user
	And I have an account

Scenario: 01. Close an Account that doesn't exist (E)
	Given I pass an id of account that doesn't exist
	When I try to close the account
	Then I will receive this core error: InvalidAccount

Scenario: 02. Close an Account already closed (E)
	Given I have a category
	And I give an id of the account Bd02 with moves
	And I already have closed the account
	When I try to close the account
	Then I will receive this core error: ClosedAccount

Scenario: 03. Close an Account that has no moves (E)
	Given I give an id of the account Bd03 without moves
	When I try to close the account
	Then I will receive this core error: CantCloseEmptyAccount
	And the account will not be closed

Scenario: 99. Close an Account with info all right (S)
	Given I have a category
	And I give an id of the account Bd99 with moves
	When I try to close the account
	Then I will receive no core error
	And the account will be closed
