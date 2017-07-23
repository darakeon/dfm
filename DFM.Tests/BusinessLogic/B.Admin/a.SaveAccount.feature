Feature: a. Creation of Account

Background:
	Given I have an active user

Scenario: 01. Save Account without name (E)
	Given I have this account to create
		| Name | Yellow | Red |
		|      |        |     |
	When I try to save the account
	Then I will receive this core error: AccountNameRequired
	And the account will not be saved

Scenario: 02. Save Account with red limit bigger than yellow limit (E)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba02 | 100    | 200 |
	When I try to save the account
	Then I will receive this core error: RedLimitAboveYellowLimit
	And the account will not be saved

Scenario: 03. Save Account with name that already exists (E)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba03 |        |     |
	And I already have this account
		| Name         | Yellow | Red |
		| Account Ba03 | 200    | 100 |
	When I try to save the account
	Then I will receive this core error: AccountAlreadyExists
	And the account will not be changed

Scenario: 04. Save Account with too big name (E)
	Given I have this account to create
		| Name                  | Yellow | Red |
		| ABCDEFGHIJKLMNOPQRSTU |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeData
	And the account will not be saved


Scenario: 95. Save Account with exactly length name (S)
	Given I have this account to create
		| Name                 | Yellow | Red |
		| ABCDEFGHIJKLMNOPQRST |        |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 96. Save Account with info all right (without limits) (S)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba96 |        |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 97. Save Account with just yellow limit (S)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba97 | 100    |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 98. Save Account with just red limit (S)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba98 |        | 100 |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 99. Save Account with info all right (with limits) (S)
	Given I have this account to create
		| Name         | Yellow | Red |
		| Account Ba99 | 200    | 100 |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved
