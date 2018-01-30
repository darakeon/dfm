Feature: Bc. Update of Account

Background:
	Given I have an active user who have accepted the contract
		And I enable Categories use

Scenario: Bc01. Change the name
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Ca01 | account_ca01 |        |     |
	When I make this changes to the account
			| Name            | Url          | Yellow | Red |
			| Ca01 - new name | account_ca01 |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Bc02. Change the name when there is moves
	Given I have this account
			| Name         | Url          | Yellow | Red |
			| Account Ca02 | account_ca02 |        |     |
		And this account has moves
	When I make this changes to the account
			| Name            | Url          | Yellow | Red |
			| Ca02 - new name | account_ca02 |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account value will not change

Scenario: Bc03. Change the url
	Given I have this account
			| Name         | Url         | Yellow | Red |
			| Account Ca03 | accountca03 |        |     |
	When I make this changes to the account
			| Name         | Url             | Yellow | Red |
			| Account Ca03 | accountca03_url |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
