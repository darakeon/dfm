Feature: Aa. Creation of User

Scenario: Aa01. Save user without e-mail (E)
	Given I have this user data
		| Email | Password | Retype Password |
		|       | password | password        |
	When I try to save the user
	Then I will receive this core error: UserEmailInvalid
	And the user will not be saved

Scenario: Aa02. Save user without password (E)
	Given I have this user data
		| Email                     | Password | Retype Password |
		| saveuser@dontflymoney.com |          |                 |
	When I try to save the user
	Then I will receive this core error: UserPasswordRequired
	And the user will not be saved

Scenario: Aa03. Save user with invalid e-mail (E)
	Given I have this user data
		| Email    | Password | Retype Password |
		| saveuser | password | password        |
	When I try to save the user
	Then I will receive this core error: UserEmailInvalid
	And the user will not be saved
	
Scenario: Aa04. Save user with repeated e-mail (E)
	Given I have this user data
		| Email                     | Password | Retype Password |
		| repeated@dontflymoney.com | password | password        |
	And I already have created this user
	When I try to save the user
	Then I will receive this core error: UserAlreadyExists
	And the user will not be changed

Scenario: Aa05. Save user too large e-mail (E)
	Given I have this user data
		| Email                                               | Password | Retype Password |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgh@dontflymoney.com | password | password        |
	When I try to save the user
	Then I will receive this core error: TooLargeData
	And the user will not be saved

Scenario: Aa06. Save user wrong retype (E)
	Given I have this user data
		| Email                  | Password | Retype Password |
		| email@dontflymoney.com | password | password_wrong  |
	When I try to save the user
	Then I will receive this core error: RetypeWrong
	And the user will not be saved


Scenario: Aa98. Save user with exactly length e-mail (S)
	Given I have this user data
		| Email                                              | Password | Retype Password |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefg@dontflymoney.com | password | password        |
	When I try to save the user
	Then I will receive no core error
	And the user will be saved

Scenario: Aa99. Save user with info all right (S)
	Given I have this user data
		| Email                     | Password | Retype Password |
		| saveuser@dontflymoney.com | password | password        |
	When I try to save the user
	Then I will receive no core error
	And the user will be saved