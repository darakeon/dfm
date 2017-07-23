Feature: h. Test security token received by e-mail

Background:
	Given I have an user
	And I have a token for its password reset
	And I have a token for its activation

Scenario: 01. Test with invalid token (E)
	Given I pass an invalid token
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: 02. Test with token of UV with action PS (E)
	Given I pass a token of UserVerification with action PasswordReset
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: 03. Test with token of PS with action UV (E)
	Given I pass a token of PasswordReset with action UserVerification
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: 98. Test with token of UV with action UV (S)
	Given I pass a token of UserVerification with action UserVerification
	When I test the token
	Then I will receive no core error

Scenario: 99. Test with token of PS with action PS (S)
	Given I pass a token of PasswordReset with action PasswordReset
	When I test the token
	Then I will receive no core error