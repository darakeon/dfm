Feature: Hg. Unsubscribe move mail

Background:
	Given test user login
		And I enable move send e-mail

Scenario: Hg01. Stop sending move mail
	Given I pass a valid UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Hg02. Stop sending move mail logged out
	Given I pass a valid UnsubscribeMoveMail token
		And I have no logged user (logoff)
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Hg03. Stop sending move mail invalid token
	Given I pass an invalid token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg04. Stop sending move mail expired token
	Given I pass an expired UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg05. Stop sending move mail inactive token
	Given I pass an inactive UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg06. Stop sending move mail with User Activate token
	Given I pass a valid UserVerification token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg07. Stop sending move mail with Password Reset token
	Given I pass a valid PasswordReset token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg08. Stop sending move mail with already used token
	Given I pass a valid UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled
	Given I enable move send e-mail
	# Same token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Hg09. Stop sending move mail reuse token
	Given I enable Categories use
		And I have a category
		And I have a move with value 20 (Out) at account Bq08
		And I have a move with value 300 (In) at account Bq08
	Then the last two e-mails will have same unsubscribe token

Scenario: Hg10. Error on stop sending move mail if user is marked for deletion
	Given I pass a valid UnsubscribeMoveMail token
		But the user is marked for deletion
	When I unsubscribe move mail
	Then I will receive this core error: UserDeleted

Scenario: Hg11. Error on stop sending move mail if user requested wipe
	Given I pass a valid UnsubscribeMoveMail token
		But the user asked data wipe
	When I unsubscribe move mail
	Then I will receive this core error: UserAskedWipe

Scenario: Hg12. Error on stop sending move mail if not signed last contract
	Given I pass a valid UnsubscribeMoveMail token
		But there is a new contract
	When I unsubscribe move mail
	Then I will receive this core error: NotSignedLastContract
