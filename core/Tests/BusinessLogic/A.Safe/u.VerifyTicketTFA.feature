﻿Feature: Au. Verify ticket two factor authentication

Background:
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I login this user
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor

Scenario: Au01. Check not verified ticket
	Given I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Au02. Check verified ticket
	Given I have a ticket key
		And I have this two-factor data
			| Secret | Code        |
			| 123    | {generated} |
		And I validate the ticket two factor
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified

Scenario: Au03. Check ticket without two factor
	Given I remove two-factor
		And I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified

Scenario: Au04. Not check ticket if user is marked for deletion
	Given I have a ticket key
		But the user is marked for deletion
	When I try to verify the ticket
	Then I will receive this core error: UserDeleted
