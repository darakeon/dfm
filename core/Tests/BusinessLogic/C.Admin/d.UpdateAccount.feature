Feature: Cd. Update account

Background:
	Given test user login

Scenario: Cd01. Change the name
	Given I already have this account
			| Name                   | Yellow | Red |
			| Account {scenarioCode} |        |     |
	When I make this changes to the account
			| Name                      | Yellow | Red |
			| {scenarioCode} - new name |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Cd02. Change the name when there are moves
	Given I have this account
			| Name                   |
			| Account {scenarioCode} |
		And this account has moves
	When I make this changes to the account
			| Name                      |
			| {scenarioCode} - new name |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account value will not change

Scenario: Cd03. Change the url if name changes
	Given I have this account
			| Name                   |
			| Account {scenarioCode} |
	When I make this changes to the account
			| Name                    |
			| Account {scenarioCode}X |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account url will be account_{scenarioCode}x

Scenario: Cd04. Change the name of another user account
	Given I have this account
			| Name                   |
			| Account {scenarioCode} |
		But there is a bad person logged in
	When I make this changes to the account
			| Name                      |
			| {scenarioCode} - new name |
		And I try to update the account
	Then I will receive this core error: InvalidAccount

Scenario: Cd05. Change the name to repeated
	Given I already have this account
			| Name                     |
			| Account {scenarioCode}.1 |
		And I already have this account
			| Name                     |
			| Account {scenarioCode}.2 |
	When I make this changes to the account
			| Name                     |
			| Account {scenarioCode}.1 |
		And I try to update the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Cd06. Not change if user is marked for deletion
	Given I already have this account
			| Name                     |
			| Account {scenarioCode}.1 |
		But the user is marked for deletion
	When I make this changes to the account
			| Name                     |
			| Account {scenarioCode}.2 |
		And I try to update the account
	Then I will receive this core error: UserDeleted

Scenario: Cd07. Not change if user requested wipe
	Given I already have this account
			| Name                     |
			| Account {scenarioCode}.1 |
		But the user asked data wipe
	When I make this changes to the account
			| Name                     |
			| Account {scenarioCode}.2 |
		And I try to update the account
	Then I will receive this core error: UserAskedWipe

Scenario: Cd08. Not change without signing contract
	Given I already have this account
			| Name                   |
			| Account {scenarioCode} |
		But there is a new contract
	When I make this changes to the account
			| Name                      |
			| {scenarioCode} - new name |
		And I try to update the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Cd09. Change the signs
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I already have this account
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 20     | 10  |
	When I make this changes to the account
			| Name                      | Yellow | Red |
			| {scenarioCode} - new name | 200    | 100 |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Cd10. Change the signs for disabled signs
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I already have this account
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 20     | 10  |
		And these settings
			| UseAccountsSigns |
			| false            |
	When I make this changes to the account
			| Name                      | Yellow | Red |
			| {scenarioCode} - new name | 200    | 100 |
		And I try to update the account
	Then I will receive this core error: UseAccountsSignsDisabled
		And the account will not be changed

Scenario: Cd11. Change the currency
	Given these settings
			| UseCurrency |
			| true             |
		And I already have this account
			| Name                   | Currency |
			| Account {scenarioCode} | EUR      |
	When I make this changes to the account
			| Name                      | Currency |
			| {scenarioCode} - new name | BRL      |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Cd12. Change the currency for disabled use currency
	Given these settings
			| UseCurrency |
			| true             |
		And I already have this account
			| Name                   | Currency |
			| Account {scenarioCode} | EUR      |
		And these settings
			| UseCurrency |
			| false       |
	When I make this changes to the account
			| Name                      | Currency |
			| {scenarioCode} - new name | BRL      |
		And I try to update the account
	Then I will receive this core error: UseCurrencyDisabled
		And the account will not be changed
