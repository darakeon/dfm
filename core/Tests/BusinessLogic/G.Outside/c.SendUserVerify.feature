﻿Feature: Gc. Send user verify

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Gc01. Send with email that doesn't exist
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of user verify
	Then I will receive this core error: InvalidUser

Scenario: Gc02. Send with info all right
	When I try to send the e-mail of user verify
	Then I will receive no core error

Scenario: Gc03. Not send if user is marked for deletion
	Given the user is marked for deletion
	When I try to send the e-mail of user verify
	Then I will receive this core error: UserDeleted

Scenario: Gc04. Not send if user requested wipe
	Given the user asked data wipe
	When I try to send the e-mail of user verify
	Then I will receive this core error: UserAskedWipe
