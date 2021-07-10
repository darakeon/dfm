﻿Feature: Bd. Close account

Background:
	Given test user login
		And I have an account
		And I disable Categories use

Scenario: Bd01. Close an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to close the account
	Then I will receive this core error: InvalidAccount

Scenario: Bd02. Close an Account already closed
	Given I give a url of the account Bd02 with moves
		And I already have closed the account
	When I try to close the account
	Then I will receive this core error: ClosedAccount

Scenario: Bd03. Close an Account that has no moves
	Given I give a url of the account Bd03 without moves
	When I try to close the account
	Then I will receive this core error: CantCloseEmptyAccount
		And the account will not be closed

Scenario: Bd04. Close an Account with info all right
	Given I give a url of the account Bd04 with moves
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date

Scenario: Bd05. Close an Account with schedule
	Given I give a url of the account Bd05 with moves
		And the account has a schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date
		And the schedule will be disabled

Scenario: Bd06. Close an Account with disabled schedule
	Given I give a url of the account Bd06 with moves
		And the account has a disabled schedule
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date

Scenario: Bd07. Close an Account with schedule that has details
	Given I give a url of the account Bd07 with moves
		And the account has a schedule with details
	When I try to close the account
	Then I will receive no core error
		And the account will be closed
		And the account will have an end date
		And the schedule will be disabled

Scenario: Bd08. Not close Account if user is marked for deletion
	Given I give a url of the account Bd08 with moves
		But the user is marked for deletion
	When I try to close the account
	Then I will receive this core error: UserDeleted

Scenario: Bd09. Not close Account if user requested wipe
	Given I give a url of the account Bd09 with moves
		But the user asked data wipe
	When I try to close the account
	Then I will receive this core error: UserAskedWipe
