﻿Feature: Ag. Reset password

Background:
	Given I have this user created
			| Email                                        | Password | Retype Password | Signed | Active |
			| passwordreset{scenarioCode}@dontflymoney.com | password | password        | true   | true   |
		And I have a ticket of this user
		And I have a token for its password reset

Scenario: Ag01. Password reset with invalid token
	Given I pass an invalid token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Ag02. Password reset with token of user verification
	Given I have a token for its activation
		And I pass a valid UserVerification token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Ag03. Password reset with no password
	Given I pass a valid PasswordReset token
		And I pass no password
	When I try to reset the password
	Then I will receive this core error: UserPasswordRequired
		And the password will not be changed

Scenario: Ag04. Password reset with info all right
	Given I pass a valid PasswordReset token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive no core error
		And the password will be changed
		And the token will not be valid anymore
		And there will be no active logins

Scenario: Ag05. Password reset with token of unsubscribe move mail
	Given I have a token for its activation
		And I pass a valid UnsubscribeMoveMail token
		And I pass this password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed
