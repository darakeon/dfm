Feature: As. Remove Two factor Authentication

Scenario: As001. With wrong password
	Given I have this user created and activated
			| Email                  | Password |
			| As001@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| As001@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| wrong    |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: As002. With all info right
	Given I have this user created and activated
			| Email                  | Password |
			| As002@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| As002@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| password |
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty
