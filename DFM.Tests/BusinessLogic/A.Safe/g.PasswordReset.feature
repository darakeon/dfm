Feature: g. Reset password of user

Background:
	Given I have an user
	And I have a token for its password reset
	And I have a token for its actvation

Scenario: 01. Password reset with invalid token (E)
	Given I pass an invalid token
	And I pass this password: new_password
	When I try to reset the password
	Then I will receive this error: InvalidToken
	And the password will not be changed

Scenario: 02. Password reset with token of user verification (E)
	Given I pass a valid UserVerification token
	And I pass this password: new_password
	When I try to reset the password
	Then I will receive this error: InvalidToken
	And the password will not be changed

Scenario: 03. Password reset with no password (E)
	Given I pass a valid PasswordReset token
	And I pass no password
	When I try to reset the password
	Then I will receive this error: UserPasswordRequired
	And the password will not be changed

Scenario: 99. Password reset with info all right (S)
	Given I pass a valid PasswordReset token
	And I pass this password: new_password
	When I try to reset the password
	Then I will receive no error
	And the password will be changed
	And the token will not be valid anymore