Feature: d. Close Account

Background:
	Given I have an user
	And I have an account

Scenario: 01. Close an Account that doesn't exist (E)
	Given I pass an id of account that doesn't exist
	When I try to close the account
	Then I will receive this error: InvalidID

Scenario: 02. Close an Account already closed (E)
	Given I close an account
	And I pass its id to close again
	When I try to close the account
	Then I will receive this error: ClosedAccount

Scenario: 03. Close an Account that has no moves (E)
	Given I give an id of account without moves
	When I try to close the account
	Then I will receive this error: CantCloseEmptyAccount
	And the account will not be closed

Scenario: 99. Close an Account with info all right (S)
	Given I give an id of account with moves
	When I try to close the account
	Then I will receive no error
	And the account will be closed
