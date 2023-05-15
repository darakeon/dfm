Feature: Ce. Close account

Background:
	Given test user login
		And I have an account
		And I disable Categories use

Scenario: Ce01. Close an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to close the account
	Then I will receive this core error: InvalidAccount

Scenario: Ce02. Close an Account already closed
	Given I give a url of the account Bd02 with moves
		And I already have closed the account
	When I try to close the account
	Then I will receive this core error: ClosedAccount

Scenario: Ce03. Close an Account that has no moves
	Given I give a url of the account Bd03 without moves
	When I try to close the account
	Then I will receive this core error: CantCloseEmptyAccount
		And the account will not be closed

Scenario: Ce04. Close an Account with info all right
	Given I give a url of the account Bd04 with moves
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date

Scenario: Ce05. Close an Account with schedule
	Given I give a url of the account Bd05 with moves
		And the account has a schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date
		And the schedule will be disabled

Scenario: Ce06. Close an Account with disabled schedule
	Given I give a url of the account Bd06 with moves
		And the account has a disabled schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date

Scenario: Ce07. Close an Account with schedule that has details
	Given I give a url of the account Bd07 with moves
		And the account has a schedule with details
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date
		And the schedule will be disabled

Scenario: Ce08. Not close Account if user is marked for deletion
	Given I give a url of the account Bd08 with moves
		But the user is marked for deletion
	When I try to close the account
	Then I will receive this core error: UserDeleted

Scenario: Ce09. Not close Account if user requested wipe
	Given I give a url of the account Bd09 with moves
		But the user asked data wipe
	When I try to close the account
	Then I will receive this core error: UserAskedWipe

Scenario: Ce10. Not close Account without signing contract
	Given I give a url of the account Bd10 with moves
		But there is a new contract
	When I try to close the account
	Then I will receive this core error: NotSignedLastContract
