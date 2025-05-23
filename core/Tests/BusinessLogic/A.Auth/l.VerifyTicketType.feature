﻿Feature: Al. Verify ticket type

Background:
	Given I have this user created
			| Email                           | Password  | Active |
			| {scenarioCode}@dontflymoney.com | pass_word | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |

Scenario: Al01. Verify ticket as none
	Given I have a ticket key
	When I try to verify the ticket type to be none
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Al02. Verify ticket as mobile
	Given I have a ticket key
	When I try to verify the ticket type to be mobile
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Al03. Verify ticket as browser
	Given I have a ticket key
	When I try to verify the ticket type to be browser
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Al04. Verify ticket as local
	Given I have a ticket key
	When I try to verify the ticket type to be local
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Al05. Verify ticket as tests
	Given I have a ticket key
	When I try to verify the ticket type to be tests
	Then I will receive no core error
		And the ticket will be verified

Scenario: Al06. Not verify if user is marked for deletion
	Given I have a ticket key
		But the user is marked for deletion
	When I try to verify the ticket type to be local
	Then I will receive this core error: UserDeleted

Scenario: Al07. Not verify if user requested wipe
	Given I have a ticket key
		But the user asked data wipe
	When I try to verify the ticket type to be local
	Then I will receive this core error: UserAskedWipe
