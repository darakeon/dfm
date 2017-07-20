Feature: e. Get user by its e-mail and password

Background: 
	Given I have an user

Scenario: 01. Validate without e-mail (E)
	Given I have this user data
		| Email | Password |
		|       | password |
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 02. Validate without password (E)
	Given I have this user data
		| Email                 | Password |
		| test@dontflymoney.com |          |
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 03. Validate with wrong e-mail (E)
	Given I have this user data
		| Email                       | Password |
		| dont_exist@dontflymoney.com | password |
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 04. Validate with wrong password (E)
	Given I have this user data
		| Email                 | Password       |
		| test@dontflymoney.com | password_wrong |
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 99. Validate with info all right (S)
	Given I have this user data
		| Email                 | Password |
		| test@dontflymoney.com | password |
	When I try to get the user
	Then I will receive no error
	And I will receive the user