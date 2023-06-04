Feature: Hd. Activate user

Background:
	Given I have this user created
			| Email                                       | Password |
			| activateuser{scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                                       | Password |
			| activateuser{scenarioCode}@dontflymoney.com | password |
		And I have a token for its activation

Scenario: Hd01. Activate user with invalid token
	Given I pass an invalid token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Hd02. Activate user with token of reset password
	Given I have a token for its password reset
		And I pass a valid PasswordReset token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Hd03. Activate user with info all right
	Given I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
		And the token will not be valid anymore

Scenario: Hd04. Activate user with token of unsubscribe move mail
	Given I have a token for its password reset
		And I pass a valid UnsubscribeMoveMail token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Hd05. Activate user with token already used
	Given I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
	Given I deactivate the user
	# Same token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Hd06. Not activate if user is marked for deletion
	Given I pass a valid UserVerification token
		But the user is marked for deletion
	When I try to activate the user
	Then I will receive this core error: UserDeleted

Scenario: Hd07. Not activate if user requested wipe
	Given I pass a valid UserVerification token
		But the user asked data wipe
	When I try to activate the user
	Then I will receive this core error: UserAskedWipe

Scenario: Hd08. Activate user with expired token
	Given I pass a valid UserVerification token
		But the token expires
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated
