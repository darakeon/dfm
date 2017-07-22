Feature: e. Delete Account

Background:
	Given I have an user
	And I have an account

Scenario: 01. Delete an Account that doesn't exist (E)
	Given I pass an id of Account that doesn't exist
	When I try to delete the account
	Then I will receive this error
		| Error     |
		| InvalidID |

Scenario: 02. Delete an Account already deleted (E)
	Given I delete an account
	And I pass its id to delete again
	When I try to delete the account
	Then I will receive this error
		| Error     |
		| InvalidID |

Scenario: 03. Delete an Account that has moves (E)
	Given I give an id of an account with moves
	When I try to delete the account
	Then I will receive this error
		| Error                      |
		| CantDeleteAccountWithMoves |
	And the account will not be deleted

Scenario: 99. Delete an Account with info all right (S)
	Given I give an id of an account without moves
	When I try to delete the account
	Then I will receive no error
	And the account will be deleted