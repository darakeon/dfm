Feature: Hf. Reset password

Background:
	Given I have this user created
			| Email                           | Password | Signed | Active |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have a ticket of this user
		And I have a token for its password reset

Scenario: Hf01. Password reset with invalid token
	Given I pass an invalid token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Hf02. Password reset with token of user verification
	Given I have a token for its activation
		And I pass a valid UserVerification token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Hf03. Password reset with no password
	Given I pass a valid PasswordReset token
		And I pass no password
	When I try to reset the password
	Then I will receive this core error: UserPasswordRequired
		And the password will not be changed

Scenario: Hf04. Password reset with info all right
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive no core error
		And the password will be changed
		And the token will not be valid anymore
		And there will be no active logins

Scenario: Hf05. Password reset with token of unsubscribe move mail
	Given I have a token for its activation
		And I pass a valid UnsubscribeMoveMail token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Hf06. Password reset with token already used
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive no core error
	# Same token
	When I try to reset the password
	Then I will receive this core error: InvalidToken

Scenario: Hf07. Not reset if user is marked for deletion
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the user is marked for deletion
	When I try to reset the password
	Then I will receive this core error: UserDeleted

Scenario: Hf08. Not reset if user requested wipe
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the user asked data wipe
	When I try to reset the password
	Then I will receive this core error: UserAskedWipe

Scenario: Hf09. Password reset with expired token
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the token expires
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed
