Feature: Ae. Create ticket

Background:

Scenario: Ae01. Validate without e-mail
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae01@dontflymoney.com | password | password        |
		And I have this user data
			| Email | Password |
			|       | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will receive no ticket

Scenario: Ae02. Validate without password
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae02@dontflymoney.com | password | password        |
		And I have this user data
			| Email                 | Password |
			| Ae02@dontflymoney.com |          |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will receive no ticket

Scenario: Ae03. Validate with wrong e-mail
	Given I have this user data
			| Email                       | Password |
			| dont_exist@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will receive no ticket

Scenario: Ae04. Validate with wrong password
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae04@dontflymoney.com | password | password        |
		And I have this user data
			| Email                 | Password       |
			| Ae04@dontflymoney.com | password_wrong |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will receive no ticket

Scenario: Ae05. Validate user disabled
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae05@dontflymoney.com | password | password        |
		And I have this user data
			| Email                 | Password |
			| Ae05@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: DisabledUser
		And I will receive no ticket

Scenario: Ae06. Disable user by excessive trying
	Given I have this user created
			| Email                 | Password | Retype Password | Active |
			| Ae06@dontflymoney.com | password | password        | true   |
		And I have this user data
			| Email                 | Password       |
			| Ae06@dontflymoney.com | password_wrong |
	When I try to get the ticket 5 times
	Then I will receive this core error: DisabledUser
		And I will receive no ticket

Scenario: Ae07. Validate disabled with wrong password
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae07@dontflymoney.com | password | password        |
		And I have this user data
			| Email | Password       |
			|       | wrong_password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
		And I will receive no ticket

Scenario: Ae08. Validate with info all right
	Given I have this user created
			| Email                 | Password | Retype Password |
			| Ae90@dontflymoney.com | password | password        |
		And I have this user data
			| Email                 | Password |
			| Ae90@dontflymoney.com | password |
		And I activate the user
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket

Scenario: Ae09. Enable user let login again
	Given I have this user created
			| Email                 | Password | Retype Password | Active |
			| Ae91@dontflymoney.com | password | password        | true   |
		And I have this user data
			| Email                 | Password       |
			| Ae91@dontflymoney.com | password_wrong |
		And I tried to get the ticket 5 times
		And I have this user data
			| Email                 | Password |
			| Ae91@dontflymoney.com | password |
		And I activate the user
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket

Scenario: Ae10. Enable user resets the password trial times
	Given I have this user created
			| Email                 | Password | Retype Password | Active |
			| Ae92@dontflymoney.com | password | password        | true   |
		And I have this user data
			| Email                 | Password       |
			| Ae92@dontflymoney.com | password_wrong |
		And I tried to get the ticket 5 times
		And I have this user data
			| Email                 | Password |
			| Ae92@dontflymoney.com | password |
		And I activate the user
		And I have this user data
			| Email                 | Password       |
			| Ae92@dontflymoney.com | password_wrong |
		And I tried to get the ticket 1 times
		And I have this user data
			| Email                 | Password |
			| Ae92@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive no core error
		And I will receive the ticket
