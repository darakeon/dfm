Feature: Be. Delete Account

Background:
	Given I have an active user
	And I have an account

Scenario: BeE01. Delete an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: BeE02. Delete an Account already deleted
	Given I give a url of the account BeE02 without moves
	And I already have deleted the account
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: BeE03. Delete an Account that has moves
	Given I have a category
	And I give a url of the account BeE03 with moves
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithMoves
	And the account will not be deleted


Scenario: BeS01. Delete an Account that had moves
	Given I have a category
	And I give a url of the account BeS01 with moves
	And I delete the moves of BeS01
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted

Scenario: BeS02. Delete an Account without moves
	Given I give a url of the account BeS02 without moves
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted

Scenario: BeS03. Delete an Account with schedule
	Given I have a category
	And I give a url of the account BeS03 without moves
	And the account has a schedule
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted