Feature: c. Get user by its e-mail and password

Background:
	Given I have an active user

Scenario: 01. Validate without e-mail (E)
	Given I dont pass the e-mail
	When I try to get the user
	Then I will receive this error
		| Error    		|
		| UserEmailRequired |
	And I will receive no user

Scenario: 02. Validate without password (E)
	Given I dont pass the password
	When I try to get the user
	Then I will receive this error
		| Error       		|
		| UserPasswordRequired |
	And I will receive no user

Scenario: 03. Validate with wrong e-mail (E)
	Given I pass an e-mail the doesn't exist
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 04. Validate with wrong password (E)
	Given I pass a wrong password
	When I try to get the user
	Then I will receive this error
		| Error       |
		| InvalidUser |
	And I will receive no user

Scenario: 99. Validate with info all right (S)
	Given I pass valid e-mail and password
	When I try to get the user
	Then I will receive no error
	And I will receive the user