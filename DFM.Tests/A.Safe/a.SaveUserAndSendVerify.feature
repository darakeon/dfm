Feature: a. Creation of User

Scenario: 01. Save user without e-mail (E)
	Given I have this user to create
		| Email | Password |
		|       | password |
	When I try to save the user
	Then I will receive this error: UserEmailInvalid
	And the user will not be saved

Scenario: 02. Save user without password (E)
	Given I have this user to create
		| Email                 | Password |
		| test@dontflymoney.com |          |
	When I try to save the user
	Then I will receive this error: UserPasswordRequired
	And the user will not be saved

Scenario: 03. Save user with invalid e-mail (E)
	Given I have this user to create
		| Email | Password |
		| test  | password |
	When I try to save the user
	Then I will receive this error: UserEmailInvalid
	And the user will not be saved
	
Scenario: 04. Save user with repeated e-mail (E)
	Given I have this user to create
		| Email                     | Password |
		| repeated@dontflymoney.com | password |
	And I already have created this user
	When I try to save the user
	Then I will receive this error: UserAlreadyExists
	And the user will not be changed

Scenario: 99. Save user with info all right (S)
	Given I have this user to create
		| Email                 | Password |
		| test@dontflymoney.com | password |
	When I try to save the user
	Then I will receive no error
	And the user will be saved