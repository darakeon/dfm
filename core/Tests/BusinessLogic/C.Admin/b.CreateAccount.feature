﻿Feature: Cb. Create account

Background:
	Given test user login

Scenario: Cb01. Save Account without name
	Given I have this account to create
			| Name | Yellow | Red |
			|      |        |     |
	When I try to save the account
	Then I will receive this core error: AccountNameRequired
		And the account will not be saved

Scenario: Cb02. Save Account with red limit bigger than yellow limit
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account to create
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 100    | 200 |
	When I try to save the account
	Then I will receive this core error: RedLimitAboveYellowLimit
		And the account will not be saved

Scenario: Cb03. Save Account with name that already exists
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		And I already have this account
			| Name                   |
			| Account {scenarioCode} |
	When I try to save the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Cb04. Save Account with too big name
	Given I have this account to create
			| Name                  |
			| ABCDEFGHIJKLMNOPQRSTU |
	When I try to save the account
	Then I will receive this core error: TooLargeAccountName
		And the account will not be saved

Scenario: Cb05. Save Account with exactly length name
	Given I have this account to create
			| Name                 |
			| ABCDEFGHIJKLMNOPQRST |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb06. Save Account with special character
	Given I have this account to create
			| Name                   |
			| Account/{scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb07. Save Account with with space
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb08. Save Account that already exists equal url
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		And I already have this account
			| Name                   |
			| Account_{scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account url will be account_{scenarioCode}_2

Scenario: Cb09. Save Account with info all right (without limits)
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb10. Save Account with just yellow limit
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account to create
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 100    |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb11. Save Account with just red limit
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account to create
			| Name                   | Yellow | Red |
			| Account {scenarioCode} |        | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb12. Save Account with both limits
	Given these settings
			| UseAccountsSigns |
			| true             |
		And I have this account to create
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 200    | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
		And the account will be open

Scenario: Cb13. Save Account with same name in another user
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
	Given there is another person logged in
		And I have this account to create
			| Name                   |
			| Account {scenarioCode} |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb14. Not save account if user is marked for deletion
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		But the user is marked for deletion
	When I try to save the account
	Then I will receive this core error: UserDeleted

Scenario: Cb15. Not save account if user requested wipe
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		But the user asked data wipe
	When I try to save the account
	Then I will receive this core error: UserAskedWipe

Scenario: Cb16. Not create account without signing contract
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		But there is a new contract
	When I try to save the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Cb17. Save Account with with limits for AccountsSign disabled
	Given these settings
			| UseAccountsSigns |
			| false            |
		And I have this account to create
			| Name                   | Yellow | Red |
			| Account {scenarioCode} | 200    | 100 |
	When I try to save the account
	Then I will receive this core error: UseAccountsSignsDisabled
		And the account will not be saved

Scenario: Cb18. Save Account with currency
	Given I have this account to create
			| Name                   | Yellow | Red | Currency |
			| Account {scenarioCode} |        |     | BRL      |
		And these settings
			| UseCurrency |
			| true        |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb19. Save Account with currency for use currency disabled
	Given I have this account to create
			| Name                   | Yellow | Red | Currency |
			| Account {scenarioCode} |        |     | BRL      |
		And these settings
			| UseCurrency |
			| false       |
	When I try to save the account
	Then I will receive this core error: UseCurrencyDisabled
		And the account will not be saved

Scenario: Cb20. Save Account without currency for use currency enabled
	Given I have this account to create
			| Name                   |
			| Account {scenarioCode} |
		And these settings
			| UseCurrency |
			| true        |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb21. Save Account above limits
	Given these limits in user plan
			| AccountOpened |
			| 3             |
		And I have these accounts
			| Name      |
			| Account 1 |
			| Account 2 |
			| Account 3 |
	Given I have this account to create
			| Name      |
			| Account 4 |
	When I try to save the account
	Then I will receive this core error: PlanLimitAccountOpenedAchieved
		And the account will not be saved

Scenario: Cb22. Save Account after close one account
	Given these limits in user plan
			| AccountOpened | AccountMonthMove |
			| 3             | 3                |
		And I have these accounts
			| Name      |
			| Account 1 |
			| Account 2 |
			| Account 3 |
		And I close the account Account 3
	Given I have this account to create
			| Name      |
			| Account 4 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
