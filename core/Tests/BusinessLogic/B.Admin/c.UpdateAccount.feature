Feature: Bc. Update account

Background:
	Given test user login
		And I disable Categories use

Scenario: Bc01. Change the name
	Given I have this account
			| Name         | Yellow | Red |
			| Account Bc01 |        |     |
	When I make this changes to the account
			| Name            | Yellow | Red |
			| Bc01 - new name |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed

Scenario: Bc02. Change the name when there are moves
	Given I have this account
			| Name         | Yellow | Red |
			| Account Bc02 |        |     |
		And this account has moves
	When I make this changes to the account
			| Name            | Yellow | Red |
			| Bc02 - new name |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account value will not change

Scenario: Bc03. Change the url if name changes
	Given I have this account
			| Name         | Yellow | Red |
			| Account Bc03 |        |     |
	When I make this changes to the account
			| Name          | Yellow | Red |
			| Account Bc03X |        |     |
		And I try to update the account
	Then I will receive no core error
		And the account will be changed
		And the account url will be account_bc03x

Scenario: Bc04. Change the name of another user account
	Given I have this account
			| Name         | Yellow | Red |
			| Account Bc04 |        |     |
		But there is a bad person logged in
	When I make this changes to the account
			| Name            | Yellow | Red |
			| Bc04 - new name |        |     |
		And I try to update the account
	Then I will receive this core error: InvalidAccount

Scenario: Bc05. Change the name to repeated
	Given I already have this account
			| Name           |
			| Account Bc05.1 |
		And I already have this account
			| Name           |
			| Account Bc05.2 |
	When I make this changes to the account
			| Name           |
			| Account Bc05.1 |
		And I try to update the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Bc06. Not change if user is marked for deletion
	Given I already have this account
			| Name           |
			| Account Bc06.1 |
		But the user is marked for deletion
	When I make this changes to the account
			| Name           |
			| Account Bc06.2 |
		And I try to update the account
	Then I will receive this core error: UserDeleted

Scenario: Bc07. Not change if user requested wipe
	Given I already have this account
			| Name           |
			| Account Bc07.1 |
		But the user asked data wipe
	When I make this changes to the account
			| Name           |
			| Account Bc07.2 |
		And I try to update the account
	Then I will receive this core error: UserAskedWipe

Scenario: Bc08. Not change without signing contract
	Given I have this account
			| Name         | Yellow | Red |
			| Account Bc08 |        |     |
		But there is a new contract
	When I make this changes to the account
			| Name            | Yellow | Red |
			| Bc08 - new name |        |     |
		And I try to update the account
	Then I will receive this core error: NotSignedLastContract
