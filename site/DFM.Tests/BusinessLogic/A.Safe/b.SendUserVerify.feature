Feature: b. Send user verify token to user

Background:
	Given I have this user created
		| Email                       | Password |
		| userverify@dontflymoney.com | password |

Scenario: 01. Send with email that doesn't exist (E)
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of user verify
	Then I will receive this core error: InvalidUser

Scenario: 99. Send with info all right (S)
	When I try to send the e-mail of user verify
	Then I will receive no core error