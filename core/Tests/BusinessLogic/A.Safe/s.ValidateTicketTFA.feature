Feature: As. Validate ticket two factor authentication

Background:
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
		And I login this user
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor

Scenario: As01. Validate with disabled two-factor
	Given I remove two-factor
		And I have this two-factor data
			| Code |
			| any  |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFANotConfigured
		And the ticket will be valid

Scenario: As02. Validate with invalid code
		But I have this two-factor data
			| Code  |
			| wrong |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFAWrongCode
		And the ticket will not be valid

Scenario: As03. Validate with valid code
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

Scenario: As04. Not validate if user is marked for deletion
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 456    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Secret | Code        |
			| 456    | {generated} |
		But the user is marked for deletion
	When I try to validate the ticket two factor
	Then I will receive this core error: UserDeleted

Scenario: As05. Not validate if user requested wipe
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 789    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Secret | Code        |
			| 789    | {generated} |
		But the user asked data wipe
	When I try to validate the ticket two factor
	Then I will receive this core error: UserAskedWipe
