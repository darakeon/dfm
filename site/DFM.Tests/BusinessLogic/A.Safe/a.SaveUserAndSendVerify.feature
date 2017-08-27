Feature: a. Creation of User

Scenario: 01. Save user without e-mail (E)
	Given I have this user data
		| Email | Password |
		|       | password |
	When I try to save the user
	Then I will receive this core error: UserEmailInvalid
	And the user will not be saved

Scenario: 02. Save user without password (E)
	Given I have this user data
		| Email                     | Password |
		| saveuser@dontflymoney.com |          |
	When I try to save the user
	Then I will receive this core error: UserPasswordRequired
	And the user will not be saved

Scenario: 03. Save user with invalid e-mail (E)
	Given I have this user data
		| Email    | Password |
		| saveuser | password |
	When I try to save the user
	Then I will receive this core error: UserEmailInvalid
	And the user will not be saved
	
Scenario: 04. Save user with repeated e-mail (E)
	Given I have this user data
		| Email                     | Password |
		| repeated@dontflymoney.com | password |
	And I already have created this user
	When I try to save the user
	Then I will receive this core error: UserAlreadyExists
	And the user will not be changed

Scenario: 05. Save user too large e-mail (E)
	Given I have this user data
		| Email                                               | Password |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgh@dontflymoney.com | password |
	When I try to save the user
	Then I will receive this core error: TooLargeData
	And the user will not be saved


Scenario: 98. Save user with exactly length e-mail (S)
	Given I have this user data
		| Email                                              | Password |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefg@dontflymoney.com | password |
	When I try to save the user
	Then I will receive no core error
	And the user will be saved

Scenario: 99. Save user with info all right (S)
	Given I have this user data
		| Email                     | Password |
		| saveuser@dontflymoney.com | password |
	When I try to save the user
	Then I will receive no core error
	And the user will be saved