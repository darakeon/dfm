Feature: Bq. Unsubscribe move mail

Background:
	Given I have a complete user logged in
		And I have an account
		And I disable Categories use
		And I enable move send e-mail

Scenario: Bq01. Stop sending move mail
	Given I give a url of the account Bq01 with moves
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Bq02. Stop sending move mail logged out
	Given I give a url of the account Bq02 with moves
		And I have no logged user (logoff)
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Bq03. Stop sending move mail invalid token
	Given I give a url of the account Bq03 with moves
		And I have no logged user (logoff)
	When I unsubscribe move mail (invalid token)
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq04. Stop sending move mail expired token
	Given I give a url of the account Bq04 with moves
		And I have no logged user (logoff)
	When I unsubscribe move mail (expired token)
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq05. Stop sending move mail inactive token
	Given I give a url of the account Bq05 with moves
		And I have no logged user (logoff)
	When I unsubscribe move mail (inactive token)
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled
