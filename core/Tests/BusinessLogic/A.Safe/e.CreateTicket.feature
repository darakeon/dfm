﻿Feature: Ae. Create ticket

Scenario: Ae01. Validate without e-mail
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email | Password |
			|       | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae02. Validate with empty password
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com |          |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae03. Validate with null password
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           |
			| {scenarioCode}@dontflymoney.com |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae04. Validate with wrong e-mail
	Given I have this user data
			| Email                       | Password |
			| dont_exist@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae05. Validate with wrong password
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae06. Validate user disabled
	Given I have this user created
			| Email                           | Password | Days |
			| {scenarioCode}@dontflymoney.com | password | -7   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: DisabledUser
		And I will not receive the ticket

Scenario: Ae07. Disable user by excessive trying
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
		And I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
	When I try to get the ticket 5 times
	Then I will receive this core error: DisabledUser
		And I will not receive the ticket

Scenario: Ae08. Validate disabled with wrong password
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email | Password       |
			|       | wrong_password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will not receive the ticket

Scenario: Ae09. Validate with info all right
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I activate the user
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket

Scenario: Ae10. Enable user let login again
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
		And I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
		And I tried to get the ticket 5 times
		And I have this user data
			| Email                 | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I activate the user
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket

Scenario: Ae11. Enable user resets the password trial times
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
		And I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
		And I tried to get the ticket 5 times
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I activate the user
		And I have this user data
			| Email                           | Password       |
			| {scenarioCode}@dontflymoney.com | password_wrong |
		And I tried to get the ticket 1 times
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket

Scenario: Ae12. Not validate if user is marked for deletion
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I activate the user
		But the user is marked for deletion
	When I try to get the ticket
	Then I will receive this core error: UserDeleted

Scenario: Ae13. Not validate if user requested wipe
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I activate the user
		But the user asked data wipe
	When I try to get the ticket
	Then I will receive this core error: UserAskedWipe

Scenario: Ae14. Let user login within 7 days after creation
	Given I have this user created
			| Email                           | Password | Active | Days |
			| {scenarioCode}@dontflymoney.com | password | true   | -6   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket
