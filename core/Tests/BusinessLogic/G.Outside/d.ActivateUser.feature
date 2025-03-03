Feature: Gd. Activate user

Background:
	Given I have this user created
			| Email                           | Password  | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have a token for its activation

Scenario: Gd01. Activate user with invalid token
	Given I pass an invalid token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Gd02. Activate user with token of reset password
	Given I have a token for its password reset
		And I pass a valid PasswordReset token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Gd03. Activate user with info all right
	Given I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
		And the token will not be valid anymore

Scenario: Gd04. Activate user with token of unsubscribe move mail
	Given I have a token for its password reset
		And I pass a valid UnsubscribeMoveMail token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Gd05. Activate user with token already used
	Given I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
	Given I deactivate the user
	# Same token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Gd06. Not activate if user is marked for deletion
	Given I pass a valid UserVerification token
		But the user is marked for deletion
	When I try to activate the user
	Then I will receive this core error: UserDeleted

Scenario: Gd07. Not activate if user requested wipe
	Given I pass a valid UserVerification token
		But the user asked data wipe
	When I try to activate the user
	Then I will receive this core error: UserAskedWipe

Scenario: Gd08. Activate user with expired token
	Given I pass a valid UserVerification token
		But the token expires
	When I try to activate the user
	Then I will receive this core error: InvalidToken
		And the user will not be activated

Scenario: Gd09. Reset Login Limit
	Given I activate the user
		But I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
	When I try to get the ticket 5 times
	Then I will receive this core error: DisabledUser
	Given I have a token for its activation
		And I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
		And the password wrong attempts will be 0

Scenario: Gd10. Reset TFA Limit
	Given I activate the user
		And I login the user
		And I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		But I have this two-factor data
			| TFA Code | Password  |
			| forgot!  | pass_word |
	When I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
	Then I will receive this core error: TFATooMuchAttempt
	Given I have a token for its activation
		And I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
		And the user will be activated
		And the tfa wrong attempts will be 0
