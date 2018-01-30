Feature: Ar. Update Two factor Authentication

Scenario: Ar01. With empty secret key
	Given I have this user created and activated
			| Email                  | Password |
			| Ar001@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| Ar001@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			|        | {generated} | password |
	When I try to set two-factor
	Then I will receive this core error: TFAEmptySecret
		And the two-factor will be empty

Scenario: Ar02. With wrong code
	Given I have this user created and activated
			| Email                  | Password |
			| Ar002@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| Ar002@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code  | Password |
			| 123    | wrong | password |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be empty

Scenario: Ar03. With wrong password
	Given I have this user created and activated
			| Email                  | Password |
			| Ar003@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| Ar003@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | wrong    |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Ar04. With all info right
	Given I have this user created and activated
			| Email                  | Password |
			| Ar004@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| Ar004@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [123]

Scenario: Ar05. Update two-factor
	Given I have this user created and activated
			| Email                  | Password |
			| Ar005@dontflymoney.com | password |
		And I login this user
			| Email                  | Password |
			| Ar005@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Secret | Code        | Password |
			| 456    | {generated} | password |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [456]
