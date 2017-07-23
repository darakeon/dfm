Feature: i. Deactivate the user

Background:
	Given I have an user

Scenario: 01. Deactivate with invalid token (E)
	Given I pass an invalid token
	When I try do deactivate the token
	Then I will receive this core error: InvalidToken

Scenario: 98. Deactivate UV with info all right (S)
	Given I have a token for its activation
	And I pass a valid UserVerification token
	When I try do deactivate the token
	Then I will receive no core error
	And the token will not be valid anymore

Scenario: 99. Deactivate PR with info all right (S)
	Given I have a token for its password reset
	And I pass a valid PasswordReset token
	When I try do deactivate the token
	Then I will receive no core error
	And the token will not be valid anymore