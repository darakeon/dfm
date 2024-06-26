﻿Feature: Ha. Test security token

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Ha01. Test with invalid token
	Given I pass an invalid token
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ha02. Test with token of UV with action PS
	Given I have a token for its activation
		And I pass a token of UserVerification with action PasswordReset
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ha03. Test with token of PS with action UV
	Given I have a token for its password reset
		And I pass a token of PasswordReset with action UserVerification
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ha04. Test with token when I created another one
	Given I have a token for its activation
		And I have a token for its password reset
		And I pass a token of UserVerification with action UserVerification
	When I test the token
	Then I will receive this core error: InvalidToken

Scenario: Ha05. Test with token of UV with action UV
	Given I have a token for its activation
		And I pass a token of UserVerification with action UserVerification
	When I test the token
	Then I will receive no core error

Scenario: Ha06. Test with token of PS with action PS
	Given I have a token for its password reset
		And I pass a token of PasswordReset with action PasswordReset
	When I test the token
	Then I will receive no core error

Scenario: Ha07. Not test if user is marked for deletion
	Given I have a token for its password reset
		And I pass a token of PasswordReset with action PasswordReset
		But the user is marked for deletion
	When I test the token
	Then I will receive this core error: UserDeleted

Scenario: Ha08. Not test if user requested wipe
	Given I have a token for its password reset
		And I pass a token of PasswordReset with action PasswordReset
		But the user asked data wipe
	When I test the token
	Then I will receive this core error: UserAskedWipe
