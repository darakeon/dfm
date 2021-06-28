Feature: Ai. Disable token

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Ai01. Disable invalid token
	Given I pass an invalid token
	When I try do disable the token
	Then I will receive this core error: InvalidToken

Scenario: Ai02. Disable UV with info all right
	Given I have a token for its activation
		And I pass a valid UserVerification token
	When I try do disable the token
	Then I will receive no core error
		And the token will not be valid anymore

Scenario: Ai03. Disable PR with info all right
	Given I have a token for its password reset
		And I pass a valid PasswordReset token
	When I try do disable the token
	Then I will receive no core error
		And the token will not be valid anymore

Scenario: Ai04. Not disable if user is marked for deletion
	Given I have a token for its activation
		And I pass a valid UserVerification token
		But the user is marked for deletion
	When I try do disable the token
	Then I will receive this core error: UserDeleted

Scenario: Ai05. Not disable if user requested wipe
	Given I have a token for its activation
		And I pass a valid UserVerification token
		But the user asked data wipe
	When I try do disable the token
	Then I will receive this core error: UserAskedWipe
