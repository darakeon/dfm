Feature: Bo. Reopen account

Background:
	Given I have a complete user logged in
		And I disable Categories use

Scenario: Bo01. Reopen an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to reopen the account
	Then I will receive this core error: InvalidAccount

Scenario: Bo02. Reopen an Account that is not closed
	Given I give a url of the account Bo02 with moves
	When I try to reopen the account
	Then I will receive this core error: OpenedAccount

Scenario: Bo03. Reopen an Account with info all right
	Given I give a url of the account Bo03 with moves
		And I already have closed the account
	When I try to reopen the account
	Then I will receive no core error
		And the account will be open
		And the account will not have an end date

Scenario: Bo04. Reopen an Account logged out
	Given I give a url of the account Bo04 with moves
		And I have no logged user (logoff)
	When I try to reopen the account
	Then I will receive this core error: Uninvited
