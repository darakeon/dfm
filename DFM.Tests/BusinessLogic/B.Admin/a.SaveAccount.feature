Feature: a. Creation of Account

Background:
	Given I have an active user

Scenario: 01. Save Account without name (E)
	Given I have this account to create
		| Name | Url          | Yellow | Red |
		|      | account_ba01 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountNameRequired
	And the account will not be saved

Scenario: 02. Save Account with red limit bigger than yellow limit (E)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba02 | account_ba02 | 100    | 200 |
	When I try to save the account
	Then I will receive this core error: RedLimitAboveYellowLimit
	And the account will not be saved

Scenario: 03. Save Account with name that already exists (E)
	Given I have this account to create
		| Name         | Url            | Yellow | Red |
		| Account Ba03 | account_ba03_2 |        |     |
	And I already have this account
		| Name         | Url          | Yellow | Red |
		| Account Ba03 | account_ba03 | 200    | 100 |
	When I try to save the account
	Then I will receive this core error: AccountAlreadyExists
	And the account will not be changed

Scenario: 04. Save Account with too big name (E)
	Given I have this account to create
		| Name                  | Url          | Yellow | Red |
		| ABCDEFGHIJKLMNOPQRSTU | account_ba04 |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeData
	And the account will not be saved


Scenario: 11. Save Account without url (E)
	Given I have this account to create
		| Name         | Url | Yellow | Red |
		| Account Ba11 |     |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlRequired
	And the account will not be saved

Scenario: 12. Save Account with url with special character (E)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba12 | account/ba12 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlInvalid
	And the account will not be saved

Scenario: 13. Save Account with url with space (E)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba13 | account ba13 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlInvalid
	And the account will not be saved

Scenario: 14. Save Account with url that already exists (E)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba14 | account_ba14 |        |     |
	And I already have this account
		| Name               | Url          | Yellow | Red |
		| Account Ba14 older | account_ba14 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlAlreadyExists
	And the account will not be changed

Scenario: 15. Save Account with url that already exists with other case (E)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba15 | account_ba15 |        |     |
	And I already have this account
		| Name               | Url          | Yellow | Red |
		| Account Ba15 older | Account_Ba15 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlAlreadyExists
	And the account will not be changed

Scenario: 16. Save Account with too big name (E)
	Given I have this account to create
		| Name         | Url                   | Yellow | Red |
		| Account Ba16 | ABCDEFGHIJKLMNOPQRSTU |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeData
	And the account will not be saved


Scenario: 91. Save Account with exactly length name (S)
	Given I have this account to create
		| Name                 | Url          | Yellow | Red |
		| ABCDEFGHIJKLMNOPQRST | account_ba91 |        |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 92. Save Account with info all right (without limits) (S)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba92 | account_ba92 |        |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 93. Save Account with just yellow limit (S)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba93 | account_ba93 | 100    |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 94. Save Account with just red limit (S)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba94 | account_ba94 |        | 100 |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved

Scenario: 95. Save Account with info all right (with limits) (S)
	Given I have this account to create
		| Name         | Url          | Yellow | Red |
		| Account Ba95 | account_ba95 | 200    | 100 |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved
	
Scenario: 96. Save Account with exactly length url (S)
	Given I have this account to create
		| Name         | Url                  | Yellow | Red |
		| Account Ba96 | ABCDEFGHIJKLMNOPQRST |        |     |
	When I try to save the account
	Then I will receive no core error
	And the account will be saved