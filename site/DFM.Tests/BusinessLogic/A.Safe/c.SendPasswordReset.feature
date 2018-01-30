Feature: Ac. Send a password reset token to user

Background:
	Given I have this user created
			| Email                              | Password | Retype Password |
			| sendpasswordreset@dontflymoney.com | password | password        |

Scenario: Ac01. Send with email that doesn't exist
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of password reset
	Then I will receive this core error: InvalidUser

Scenario: Ac02. Send with info all right
	When I try to send the e-mail of password reset
	Then I will receive no core error
