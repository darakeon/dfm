Feature: At. Validate Ticket Two factor Authentication

Background:
	Given I have this user created and activated
			| Email               | Password |
			| At@dontflymoney.com | password |
		And I login this user
			| Email               | Password |
			| At@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor

Scenario: At01. Validate with disabled two-factor
	Given I remove two-factor
		And I have this two-factor data
			| Code |
			| any  |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFANotConfigured
		And the ticket will be valid

Scenario: At02. Validate with invalid code
		But I have this two-factor data
			| Code  |
			| wrong |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFAWrongCode
		And the ticket will not be valid

Scenario: At03. Validate with valid code
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 456    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Secret | Code        |
			| 456    | {generated} |
	When I try to validate the ticket two factor
	Then I will receive no core error
		And the ticket will be valid
