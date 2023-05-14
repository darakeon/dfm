Feature: Aq. Update two factor authentication

Background:
	Given I have this user created
			| Email                           | Password | Active | Signed |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I login this user
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Aq01. With empty secret key
	Given I have this two-factor data
			| Secret | Code        | Password |
			|        | {generated} | password |
	When I try to set two-factor
	Then I will receive this core error: TFAEmptySecret
		And the two-factor will be empty

Scenario: Aq02. With wrong code
	Given I have this two-factor data
			| Secret | Code  | Password |
			| 123    | wrong | password |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be empty

Scenario: Aq03. With wrong password
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | wrong    |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Aq04. With all info right
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [123]

Scenario: Aq05. Update two-factor
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor
		And I have this two-factor data
			| Secret | Code        | Password |
			| 456    | {generated} | password |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [456]

Scenario: Aq06. With empty password
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} |          |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Aq07. With null password
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | {null}   |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be empty

Scenario: Aq08. Not update if user is marked for deletion
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		But the user is marked for deletion
	When I try to set two-factor
	Then I will receive this core error: UserDeleted

Scenario: Aq09. Not update if user requested wipe
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		But the user asked data wipe
	When I try to set two-factor
	Then I will receive this core error: UserAskedWipe

Scenario: Aq10. Not update if not signed last contract
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		But there is a new contract
	When I try to set two-factor
	Then I will receive this core error: NotSignedLastContract
