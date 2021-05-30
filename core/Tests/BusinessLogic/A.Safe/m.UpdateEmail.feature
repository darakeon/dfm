﻿Feature: Am. Update e-mail

Background:
	Given I have this user created
			| Email                           | Password | Active | Signed |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have a ticket of this user

Scenario: Am01. E-mail change with wrong current password
	Given I pass this new e-mail and password
			| New E-mail             | Current Password |
			| Am01_@dontflymoney.com | password_wrong   |
	When I try to change the e-mail
	Then I will receive this core error: WrongPassword
		And the e-mail will not be changed
		And the user will be activated

Scenario: Am02. E-mail change with empty new e-mail
	Given I pass this new e-mail and password
			| New E-mail | Current Password |
			|            | password         |
	When I try to change the e-mail
	Then I will receive this core error: UserEmailRequired
		And the e-mail will not be changed
		And the user will be activated

Scenario: Am03. E-mail change with info all right
	Given I pass this new e-mail and password
			| New E-mail             | Current Password |
			| Am03_@dontflymoney.com | password         |
	When I try to change the e-mail
	Then I will receive no core error
		And the e-mail will be changed
		And the user will not be activated

Scenario: Am04. Not update if user is marked for deletion
	Given I pass this new e-mail and password
			| New E-mail             | Current Password |
			| Am04_@dontflymoney.com | password         |
		But the user is marked for deletion
	When I try to change the e-mail
	Then I will receive this core error: UserDeleted
