Feature: e. Validate User and get Ticket

Background: 
	Given I have this user to create
		| Email                     | Password |
		| validate@dontflymoney.com | password |

Scenario: 01. Validate without e-mail (E)
	Given I have this user data
		| Email | Password |
		|       | password |
	When I try to get the user
	Then I will receive this core error: InvalidUser
	And I will receive no user

Scenario: 02. Validate without password (E)
	Given I have this user data
		| Email                     | Password |
		| validate@dontflymoney.com |          |
	When I try to get the user
	Then I will receive this core error: InvalidUser
	And I will receive no user

Scenario: 03. Validate with wrong e-mail (E)
	Given I have this user data
		| Email                       | Password |
		| dont_exist@dontflymoney.com | password |
	When I try to get the user
	Then I will receive this core error: InvalidUser
	And I will receive no user

Scenario: 04. Validate with wrong password (E)
	Given I have this user data
		| Email                     | Password       |
		| validate@dontflymoney.com | password_wrong |
	When I try to get the user
	Then I will receive this core error: InvalidUser
	And I will receive no user

Scenario: 05. Validate user disabled (S)
	Given I have this user data
		| Email                     | Password |
		| validate@dontflymoney.com | password |
	When I try to get the user
	Then I will receive this core error: DisabledUser
	And I will receive no user

Scenario: 99. Validate with info all right (S)
	Given I have this user data
		| Email                     | Password |
		| validate@dontflymoney.com | password |
	And I activate the user
	When I try to get the user
	Then I will receive no core error
	And I will receive the user