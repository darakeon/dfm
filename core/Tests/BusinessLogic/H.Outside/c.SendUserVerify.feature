Feature: Hc. Send user verify

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Hc01. Send with email that doesn't exist
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of user verify
	Then I will receive this core error: InvalidUser

Scenario: Hc02. Send with info all right
	When I try to send the e-mail of user verify
	Then I will receive no core error

Scenario: Hc03. Not send if user is marked for deletion
	Given the user is marked for deletion
	When I try to send the e-mail of user verify
	Then I will receive this core error: UserDeleted

Scenario: Hc04. Not send if user requested wipe
	Given the user asked data wipe
	When I try to send the e-mail of user verify
	Then I will receive this core error: UserAskedWipe
