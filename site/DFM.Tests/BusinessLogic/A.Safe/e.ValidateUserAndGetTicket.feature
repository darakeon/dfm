Feature: e. Validate User and get Ticket

Background: 

Scenario: 01. Validate without e-mail (E)
	Given I have this user created
		| Email                 | Password | Retype Password |
		| Ae01@dontflymoney.com | password | password        |
	And I have this user data
		| Email | Password |
		|       | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
	And I will receive no ticket

Scenario: 02. Validate without password (E)
	Given I have this user created
		| Email                 | Password | Retype Password |
		| Ae02@dontflymoney.com | password | password        |
	And I have this user data
		| Email                 | Password |
		| Ae02@dontflymoney.com |          |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
	And I will receive no ticket

Scenario: 03. Validate with wrong e-mail (E)
	Given I have this user data
		| Email                       | Password |
		| dont_exist@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
	And I will receive no ticket

Scenario: 04. Validate with wrong password (E)
	Given I have this user created
		| Email                 | Password | Retype Password |
		| Ae04@dontflymoney.com | password | password        |
	And I have this user data
		| Email                 | Password       |
		| Ae04@dontflymoney.com | password_wrong |
	When I try to get the ticket
	Then I will receive this core error: InvalidUser
	And I will receive no ticket

Scenario: 05. Validate user disabled (E)
	Given I have this user created
		| Email                 | Password | Retype Password |
		| Ae05@dontflymoney.com | password | password        |
	And I have this user data
		| Email                 | Password |
		| Ae05@dontflymoney.com | password |
	When I try to get the ticket
	Then I will receive this core error: DisabledUser
	And I will receive no ticket

Scenario: 06. Disable user by excessive trying (E)
	Given I have this user created and activated
		| Email                 | Password | Retype Password |
		| Ae06@dontflymoney.com | password | password        |
	And I have this user data
		| Email                 | Password       |
		| Ae06@dontflymoney.com | password_wrong |
	When I try to get the ticket 5 times
	Then I will receive this core error: DisabledUser
	And I will receive no ticket

Scenario: 90. Validate with info all right (S)
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

Scenario: 91. Enable user let login again (S)
	Given I have this user created and activated
		| Email                 | Password | Retype Password |
		| Ae91@dontflymoney.com | password | password        |
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

Scenario: 92. Enable user resets the password trial times (S)
	Given I have this user created and activated
		| Email                 | Password | Retype Password |
		| Ae92@dontflymoney.com | password | password        |
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
