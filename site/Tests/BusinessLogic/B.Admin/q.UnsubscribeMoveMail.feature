Feature: Bq. Unsubscribe move mail

Background:
	Given I have a complete user logged in
		And I have an account
		And I disable Categories use
		And I enable move send e-mail

Scenario: Bq01. Stop sending move mail
	Given I pass a valid UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Bq02. Stop sending move mail logged out
	Given I pass a valid UnsubscribeMoveMail token
		And I have no logged user (logoff)
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Bq03. Stop sending move mail invalid token
	Given I pass an invalid token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq04. Stop sending move mail expired token
	Given I pass an expired UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq05. Stop sending move mail inactive token
	Given I pass an inactive UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq06. Stop sending move mail with User Activate token
	Given I pass a valid UserVerification token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Bq07. Stop sending move mail with Password Reset token
	Given I pass a valid PasswordReset token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled
