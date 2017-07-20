Feature: f. Send a password reset token to user

Background:
	Given I have an user

Scenario: 01. Send with email that doesn't exist (E)
	Given I pass an e-mail the doesn't exist
	When I try to send the e-mail of password reset
	Then I will receive this error
		| Error       |
		| InvalidUser |

Scenario: 99. Send with info all right (S)
	Given I pass valid e-mail
	When I try to send the e-mail of password reset
	Then I will receive no error