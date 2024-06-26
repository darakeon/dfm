﻿Feature: Cg. Delete account

Background:
	Given test user login
		And I have an account
		And these settings
			| UseCategories |
			| false         |

Scenario: Cg01. Delete an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: Cg02. Delete an Account already deleted
	Given I give a url of the account Be02 without moves
		And I already have deleted the account
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: Cg03. Delete an Account that has moves
	Given I give a url of the account Be03 with moves
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithMoves
		And the account will not be deleted

Scenario: Cg04. Delete an Account with schedule
	Given I give a url of the account Be04 without moves
		And the account has a schedule
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithSchedules
		And the account will not be deleted

Scenario: Cg05. Delete an Account with detailed schedule
	Given I give a url of the account Be05 without moves
		And the account has a schedule with details
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithSchedules
		And the account will not be deleted

Scenario: Cg06. Delete an Account that had moves
	Given I give a url of the account Be06 with moves
		And I delete the moves of Be06
	When I try to delete the account
	Then I will receive no core error
		And the account will be deleted

Scenario: Cg07. Delete an Account without moves
	Given I give a url of the account Be07 without moves
	When I try to delete the account
	Then I will receive no core error
		And the account will be deleted

Scenario: Cg08. Delete an Account with schedule
	Given I give a url of the account Be08 without moves
		And the account has a disabled schedule
	When I try to delete the account
	Then I will receive no core error
		And the account will be deleted

Scenario: Cg09. Delete an Account with detailed schedule
	Given I give a url of the account Be09 without moves
		And the account has a disabled schedule with details
	When I try to delete the account
	Then I will receive no core error
		And the account will be deleted

Scenario: Cg10. Not delete Account if user is marked for deletion
	Given I give a url of the account Be10 without moves
		But the user is marked for deletion
	When I try to delete the account
	Then I will receive this core error: UserDeleted

Scenario: Cg11. Not delete Account if user requested wipe
	Given I give a url of the account Be11 without moves
		But the user asked data wipe
	When I try to delete the account
	Then I will receive this core error: UserAskedWipe

Scenario: Cg12. Not delete Account without signing contract
	Given I give a url of the account Be12 without moves
		But there is a new contract
	When I try to delete the account
	Then I will receive this core error: NotSignedLastContract
