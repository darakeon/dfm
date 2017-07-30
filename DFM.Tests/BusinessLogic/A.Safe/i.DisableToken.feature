Feature: i. Disable the token

Background:
	Given I have this user to create
		| Email                         | Password |
		| disabletoken@dontflymoney.com | password |

Scenario: 01. Disable invalid token (E)
	Given I pass an invalid token
	When I try do disable the token
	Then I will receive this core error: InvalidToken

Scenario: 98. Disable UV with info all right (S)
	Given I have a token for its activation
	And I pass a valid UserVerification token
	When I try do disable the token
	Then I will receive no core error
	And the token will not be valid anymore

Scenario: 99. Disable PR with info all right (S)
	Given I have a token for its password reset
	And I pass a valid PasswordReset token
	When I try do disable the token
	Then I will receive no core error
	And the token will not be valid anymore