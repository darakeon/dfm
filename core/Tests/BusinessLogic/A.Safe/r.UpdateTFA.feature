Feature: Ar. Update two factor authentication

Scenario: Ar01. With empty secret key
	Given I have this user created
			| Email                 | Password | Active |
			| Ar01@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar01@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			|        | {generated} | password |
	When I try to set two-factor
	Then I will receive this core error: TFAEmptySecret
		And the two-factor will be empty

Scenario: Ar02. With wrong code
	Given I have this user created
			| Email                 | Password | Active |
			| Ar02@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar02@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code  | Password |
			| 123    | wrong | password |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be empty

Scenario: Ar03. With wrong password
	Given I have this user created
			| Email                 | Password | Active |
			| Ar03@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar03@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | wrong    |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Ar04. With all info right
	Given I have this user created
			| Email                 | Password | Active |
			| Ar04@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar04@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [123]

Scenario: Ar05. Update two-factor
	Given I have this user created
			| Email                 | Password | Active |
			| Ar05@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar05@dontflymoney.com | password |
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

Scenario: Ar06. With empty password
	Given I have this user created
			| Email                 | Password | Active |
			| Ar06@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar06@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} |          |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Ar07. With null password
	Given I have this user created
			| Email                 | Password | Active |
			| Ar03@dontflymoney.com | password | true   |
		And I login this user
			| Email                 | Password |
			| Ar03@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | {null}   |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty
