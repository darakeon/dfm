﻿Feature: Ak. Verify ticket two factor authentication

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor

Scenario: Ak01. Check not verified ticket
	Given I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Ak02. Check verified ticket
	Given I have a ticket key
		And I have this two-factor data
			| Secret | TFA Code    |
			| 123    | {generated} |
		And I validate the ticket two factor
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified

Scenario: Ak03. Check ticket without two factor
	Given I remove two-factor
		And I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified

Scenario: Ak04. Not check ticket if user is marked for deletion
	Given I have a ticket key
		But the user is marked for deletion
	When I try to verify the ticket
	Then I will receive this core error: UserDeleted

Scenario: Ak05. Not check ticket if user requested wipe
	Given I have a ticket key
		But the user asked data wipe
	When I try to verify the ticket
	Then I will receive this core error: UserAskedWipe
