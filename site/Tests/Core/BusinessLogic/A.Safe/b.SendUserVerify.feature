Feature: Ab. Send user verify

Background:
	Given I have this user created
			| Email                       | Password | Retype Password |
			| userverify@dontflymoney.com | password | password        |

Scenario: Ab01. Send with email that doesn't exist
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of user verify
	Then I will receive this core error: InvalidUser

Scenario: Ab02. Send with info all right
	When I try to send the e-mail of user verify
	Then I will receive no core error
