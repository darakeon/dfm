﻿Feature: Ge. Send password reset

Background:
	Given I have this user created
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this user data
			| Email                           |
			| {scenarioCode}@dontflymoney.com |

# if you answer that the user do not exists,
# someone could use this method to enumerate users in DB
Scenario: Ge01. Send with email that doesn't exist
	Given I pass an e-mail that doesn't exist
	When I try to send the e-mail of password reset
	Then I will receive no core error

Scenario: Ge02. Send with info all right
	When I try to send the e-mail of password reset
	Then I will receive no core error

Scenario: Ge03. Not send if user is marked for deletion
	Given the user is marked for deletion
	When I try to send the e-mail of password reset
	Then I will receive this core error: UserDeleted

Scenario: Ge04. Not send if user requested wipe
	Given the user asked data wipe
	When I try to send the e-mail of password reset
	Then I will receive this core error: UserAskedWipe
