﻿Feature: Al. Change password

Background:
	Given I have this user created
			| Email                           | Password | Active | Signed |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have a ticket of this user

Scenario: Al01. Password change with wrong current password
	Given I pass this password
			| Current Password | Password     | Retype Password |
			| password_wrong   | new_password | new_password    |
	When I try to change the password
	Then I will receive this core error: WrongPassword
		And the password will not be changed
		And the ticket will still be valid

Scenario: Al02. Password change with empty new password
	Given I pass this password
			| Current Password | Password | Retype Password |
			| password         |          | new_password    |
	When I try to change the password
	Then I will receive this core error: UserPasswordRequired
		And the password will not be changed
		And the ticket will still be valid

Scenario: Al03. Password change with different retype password
	Given I pass this password
			| Current Password | Password     | Retype Password |
			| password         | new_password | password_wrong  |
	When I try to change the password
	Then I will receive this core error: RetypeWrong
		And the password will not be changed
		And the ticket will still be valid

Scenario: Al04. Password change with info all right
	Given I pass this password
			| Current Password | Password     | Retype Password |
			| password         | new_password | new_password    |
	When I try to change the password
	Then I will receive no core error
		And the password will be changed
		And only the last login will be active
