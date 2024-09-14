Feature: Gg. Unsubscribe move mail

Background:
	Given test user login
		And these settings
			| SendMoveEmail |
			| true          |

Scenario: Gg01. Stop sending move mail
	Given I pass a valid UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Gg02. Stop sending move mail logged out
	Given I pass a valid UnsubscribeMoveMail token
		And I have no logged user (logoff)
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled

Scenario: Gg03. Stop sending move mail invalid token
	Given I pass an invalid token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg04. Stop sending move mail expired token
	Given I pass an expired UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg05. Stop sending move mail inactive token
	Given I pass an inactive UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg06. Stop sending move mail with User Activate token
	Given I pass a valid UserVerification token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg07. Stop sending move mail with Password Reset token
	Given I pass a valid PasswordReset token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg08. Stop sending move mail with already used token
	Given I pass a valid UnsubscribeMoveMail token
	When I unsubscribe move mail
	Then I will receive no core error
		And the move mail will not be enabled
	Given these settings
			| SendMoveEmail |
			| true          |
	# Same token
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled

Scenario: Gg09. Stop sending move mail reuse token
	Given these settings
			| UseCategories |
			| true          |
		And I have a category
		And I have a move with value 20 (Out) at account Bq08
		And I have a move with value 300 (In) at account Bq08
	Then the last two e-mails will have same unsubscribe token

Scenario: Gg10. Error on stop sending move mail if user is marked for deletion
	Given I pass a valid UnsubscribeMoveMail token
		But the user is marked for deletion
	When I unsubscribe move mail
	Then I will receive this core error: UserDeleted

Scenario: Gg11. Error on stop sending move mail if user requested wipe
	Given I pass a valid UnsubscribeMoveMail token
		But the user asked data wipe
	When I unsubscribe move mail
	Then I will receive this core error: UserAskedWipe

Scenario: Gg12. Error on stop sending move mail if not signed last contract
	Given I pass a valid UnsubscribeMoveMail token
		But there is a new contract
	When I unsubscribe move mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Gg13. Stop sending move mail with expired token
	Given I pass a valid UnsubscribeMoveMail token
		But the token expires
	When I unsubscribe move mail
	Then I will receive this core error: InvalidToken
		And the move mail will be enabled
