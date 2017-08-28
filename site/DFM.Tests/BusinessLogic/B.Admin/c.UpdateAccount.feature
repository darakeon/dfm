Feature: c. Update of Account

Background:
	Given I have an active user

Scenario: 01. Change the name (S)
	Given I have this account
		| Name         | Url          | Yellow | Red |
		| Account Ca01 | account_ca01 |        |     |
	When I make this changes to the account
		| Name            | Url          | Yellow | Red |
		| Ca01 - new name | account_ca01 |        |     |
	And I try to update the account
	Then I will receive no core error
	And the account will be changed
	
Scenario: 02. Change the name when there is moves (S)
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


Scenario: 03. Change the url (S)
	Given I have this account
		| Name         | Url         | Yellow | Red |
		| Account Ca03 | accountca03 |        |     |
	When I make this changes to the account
		| Name         | Url             | Yellow | Red |
		| Account Ca03 | accountca03_url |        |     |
	And I try to update the account
	Then I will receive no core error
	And the account will be changed
