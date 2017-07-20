Feature: a. Creation of User

Scenario: 01. Save user without e-mail (E)
	Given I have this user to create
		| Email | Password |
		|       | testDFM  |
	When I try to save the user
	Then I will receive this error
		| Error             |
		| UserEmailRequired |
	And the user will not be saved

Scenario: 02. Save user without password (E)
	Given I have this user to create
		| Email                 | Password |
		| test@dontflymoney.com |          |
	When I try to save the user
	Then I will receive this error
		| Error                |
		| UserPasswordRequired |
	And the user will not be saved

Scenario: 03. Save user with invalid e-mail (E)
	Given I have this user to create
		| Email | Password |
		| test  | testDFM  |
	When I try to save the user
	Then I will receive this error
		| Error            |
		| UserEmailInvalid |
	And the user will not be saved

Scenario: 03. Save user with repeated e-mail (E)
	Given I have this user to create
		| Email                 | Password |
		| test@dontflymoney.com | testDFM  |
	And I already have created this user
	When I try to save the user
	Then I will receive this error
		| Error             |
		| UserAlreadyExists |
	And the user will not be saved

Scenario: 99. Save user with info all right (S)
	Given I have this user to create
		| Email                 | Password |
		| test@dontflymoney.com | testDFM  |
	When I try to save the user
	Then I will receive no error
	And the user will be saved