Feature: e. Delete Account

Background:
	Given I have an active user
	And I have an account

Scenario: 01. Delete an Account that doesn't exist (E)
	Given I pass a name of account that doesn't exist
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: 02. Delete an Account already deleted (E)
	Given I give a name of the account Be02 without moves
	And I already have deleted the account
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: 03. Delete an Account that has moves (E)
	Given I have a category
	And I give a name of the account Be03 with moves
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithMoves
	And the account will not be deleted

Scenario: 99. Delete an Account with info all right (S)
	Given I give a name of the account Be99 without moves
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted