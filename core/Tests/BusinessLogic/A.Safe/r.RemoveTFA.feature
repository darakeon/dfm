Feature: Ar. Remove two factor authentication

Scenario: Ar01. With wrong password
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
		And I have this two-factor data
			| Password |
			| wrong    |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ar02. With all info right
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
		And I have this two-factor data
			| Password |
			| password |
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty

Scenario: Ar03. With empty password
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
		And I have this two-factor data
			| Password |
			|          |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ar04. With null password
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
		And I have this two-factor data
			| Password |
			| {null}   |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ar05. Not remove if user is marked for deletion
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
		And I have this two-factor data
			| Password |
			| password |
		But the user is marked for deletion
	When I try to remove two-factor
	Then I will receive this core error: UserDeleted

Scenario: Ar06. Not remove if user requested wipe
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
		And I have this two-factor data
			| Password |
			| password |
		But the user asked data wipe
	When I try to remove two-factor
	Then I will receive this core error: UserAskedWipe
