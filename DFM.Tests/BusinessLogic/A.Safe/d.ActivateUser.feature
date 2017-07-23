Feature: d. Activate the user

Background:
	Given I have an user
	And I have a token for its activation
	And I have a token for its password reset

Scenario: 01. Activate user with invalid token (E)
	Given I pass an invalid token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
	And the user will not be activated

Scenario: 02. Activate user with token of reset password (E)
	Given I pass a valid PasswordReset token
	When I try to activate the user
	Then I will receive this core error: InvalidToken
	And the user will not be activated

Scenario: 99. Activate user with info all right (S)
	Given I pass a valid UserVerification token
	When I try to activate the user
	Then I will receive no core error
	And the user will be activated
	And the token will not be valid anymore