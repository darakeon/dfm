Feature: Ah. Test security token received by e-mail

Background:
	Given I have this user created
		| Email                              | Password | Retype Password |
		| testsecuritytoken@dontflymoney.com | password | password        |

Scenario: Ah01. Test with invalid token (E)
	Given I pass an invalid token
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ah02. Test with token of UV with action PS (E)
	Given I have a token for its activation
	And I pass a token of UserVerification with action PasswordReset
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ah03. Test with token of PS with action UV (E)
	Given I have a token for its password reset
	And I pass a token of PasswordReset with action UserVerification
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ah04. Test with token when I created another one (E)
	Given I have a token for its activation
	And I have a token for its password reset
	And I pass a token of UserVerification with action UserVerification
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ah90. Test with token of UV with action UV (S)
	Given I have a token for its activation
	And I pass a token of UserVerification with action UserVerification
	When I test the token
	Then I will receive no core error

Scenario: Ah91. Test with token of PS with action PS (S)
	Given I have a token for its password reset
	And I pass a token of PasswordReset with action PasswordReset
	When I test the token
	Then I will receive no core error