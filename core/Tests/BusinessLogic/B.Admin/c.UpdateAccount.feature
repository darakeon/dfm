Feature: Bc. Update account

Background:
	Given test user login
		And I disable Categories use

Scenario: Bc01. Change the name
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Bc01 | account_bc01 |        |     |
	When I make this changes to the account
			| Name            | Url          | Yellow | Red |
			| Bc01 - new name | account_bc01 |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Bc02. Change the name when there are moves
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Bc02 | account_bc02 |        |     |
		And this account has moves
	When I make this changes to the account
			| Name            | Url          | Yellow | Red |
			| Bc02 - new name | account_bc02 |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account value will not change

Scenario: Bc03. Change the url
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Bc03 | account_bc03 |        |     |
	When I make this changes to the account
			| Name         | Url              | Yellow | Red |
			| Account Bc03 | account_bc03_url |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Bc04. Change the name of another user account
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Bc04 | account_bc04 |        |     |
		But there is a bad person logged in
	When I make this changes to the account
			| Name            | Url          | Yellow | Red |
			| Bc04 - new name | account_bc04 |        |     |
		And I try to update the account
	Then I will receive this core error: InvalidAccount

Scenario: Bc05. Change the url of another user account
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Bc05 | account_bc05 |        |     |
		But there is a bad person logged in
	When I make this changes to the account
			| Name         | Url              | Yellow | Red |
			| Account Bc05 | account_bc05_url |        |     |
		And I try to update the account
	Then I will receive this core error: InvalidAccount

Scenario: Bc06. Change the name to repeated
	Given I already have this account
			| Name           | Url            |
			| Account Bc06.1 | account_bc06_1 |
		And I already have this account
			| Name           | Url            |
			| Account Bc06.2 | account_bc06_2 |
	When I make this changes to the account
			| Name           | Url            |
			| Account Bc06.1 | account_bc06_2 |
		And I try to update the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Bc07. Change the url to repeated
	Given I already have this account
			| Name           | Url            |
			| Account Bc07.1 | account_bc07_1 |
		And I already have this account
			| Name           | Url            |
			| Account Bc07.2 | account_bc07_2 |
	When I make this changes to the account
			| Name           | Url            |
			| Account Bc07.2 | account_bc07_1 |
		And I try to update the account
	Then I will receive this core error: AccountUrlAlreadyExists
		And the account will not be changed

Scenario: Bc08. Not change if user is marked for deletion
	Given I already have this account
			| Name           | Url            |
			| Account Bc08.1 | account_bc08_1 |
		But the user is marked for deletion
	When I make this changes to the account
			| Name           | Url            |
			| Account Bc08.2 | account_bc08_1 |
		And I try to update the account
	Then I will receive this core error: UserDeleted

Scenario: Bc09. Not change if user requested wipe
	Given I already have this account
			| Name           | Url            |
			| Account Bc09.1 | account_bc09_1 |
		But the user asked data wipe
	When I make this changes to the account
			| Name           | Url            |
			| Account Bc09.2 | account_bc09_1 |
		And I try to update the account
	Then I will receive this core error: UserAskedWipe
