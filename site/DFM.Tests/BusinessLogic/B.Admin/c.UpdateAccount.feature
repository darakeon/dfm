Feature: Bc. Update of Account

Background:
	Given I have an active user
		And the user have accepted the contract
		And I enable Categories use

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

Scenario: Bc02. Change the name when there is moves
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
