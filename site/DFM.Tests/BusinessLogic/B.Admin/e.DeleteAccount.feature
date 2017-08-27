Feature: e. Delete Account

Background:
	Given I have an active user
	And I have an account

Scenario: 01. Delete an Account that doesn't exist (E)
	Given I pass a url of account that doesn't exist
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: 02. Delete an Account already deleted (E)
	Given I give a url of the account Be02 without moves
	And I already have deleted the account
	When I try to delete the account
	Then I will receive this core error: InvalidAccount

Scenario: 03. Delete an Account that has moves (E)
	Given I have a category
	And I give a url of the account Be03 with moves
	When I try to delete the account
	Then I will receive this core error: CantDeleteAccountWithMoves
	And the account will not be deleted


Scenario: 98. Delete an Account that had moves (S)
	Given I have a category
	And I give a url of the account Be98 with moves
	And I delete the moves of Be98
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted

Scenario: 99. Delete an Account without moves (S)
	Given I give a url of the account Be99 without moves
	When I try to delete the account
	Then I will receive no core error
	And the account will be deleted