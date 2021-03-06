﻿Feature: Ba. Create account

Background:
	Given test user login

Scenario: Ba01. Save Account without name
	Given I have this account to create
			| Name | Url          | Yellow | Red |
			|      | account_ba01 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountNameRequired
		And the account will not be saved

Scenario: Ba02. Save Account with red limit bigger than yellow limit
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba02 | account_ba02 | 100    | 200 |
	When I try to save the account
	Then I will receive this core error: RedLimitAboveYellowLimit
		And the account will not be saved

Scenario: Ba03. Save Account with name that already exists
	Given I have this account to create
			| Name         | Url            | Yellow | Red |
			| Account Ba03 | account_ba03_2 |        |     |
		And I already have this account
			| Name         | Url          | Yellow | Red |
			| Account Ba03 | account_ba03 | 200    | 100 |
	When I try to save the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Ba04. Save Account with too big name
	Given I have this account to create
			| Name                  | Url          | Yellow | Red |
			| ABCDEFGHIJKLMNOPQRSTU | account_ba04 |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeAccountName
		And the account will not be saved

Scenario: Ba05. Save Account without url
	Given I have this account to create
			| Name         | Url | Yellow | Red |
			| Account Ba11 |     |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlRequired
		And the account will not be saved

Scenario: Ba06. Save Account with url with special character
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba12 | account/ba12 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlInvalid
		And the account will not be saved

Scenario: Ba07. Save Account with url with space
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba13 | account ba13 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlInvalid
		And the account will not be saved

Scenario: Ba08. Save Account with url that already exists
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba14 | account_ba14 |        |     |
		And I already have this account
			| Name               | Url          | Yellow | Red |
			| Account Ba14 older | account_ba14 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlAlreadyExists
		And the account will not be changed

Scenario: Ba09. Save Account with url that already exists with other case
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba15 | account_ba15 |        |     |
		And I already have this account
			| Name               | Url          | Yellow | Red |
			| Account Ba15 older | Account_Ba15 |        |     |
	When I try to save the account
	Then I will receive this core error: AccountUrlAlreadyExists
		And the account will not be changed

Scenario: Ba10. Save Account with too big name
	Given I have this account to create
			| Name         | Url                   | Yellow | Red |
			| Account Ba16 | ABCDEFGHIJKLMNOPQRSTU |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeAccountUrl
		And the account will not be saved

Scenario: Ba11. Save Account with exactly length name
	Given I have this account to create
			| Name                 | Url          | Yellow | Red |
			| ABCDEFGHIJKLMNOPQRST | account_ba91 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba12. Save Account with info all right (without limits)
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba92 | account_ba92 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba13. Save Account with just yellow limit
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba93 | account_ba93 | 100    |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba14. Save Account with just red limit
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba94 | account_ba94 |        | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba15. Save Account with info all right (with limits)
	Given I have this account to create
			| Name         | Url          | Yellow | Red |
			| Account Ba95 | account_ba95 | 200    | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
		And the account will be open

Scenario: Ba16. Save Account with exactly length url
	Given I have this account to create
			| Name         | Url                  | Yellow | Red |
			| Account Ba96 | ABCDEFGHIJKLMNOPQRST |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba17. Save Account with same name and url in another user
	Given I have this account to create
			| Name         | Url  | Yellow | Red |
			| Account Ba17 | ba17 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
	Given there is another person logged in
		And I have this account to create
			| Name         | Url  | Yellow | Red |
			| Account Ba17 | ba17 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Ba18. Not save account if user is marked for deletion
	Given I have this account to create
			| Name         | Url  | Yellow | Red |
			| Account Ba18 | ba18 |        |     |
		But the user is marked for deletion
	When I try to save the account
	Then I will receive this core error: UserDeleted

Scenario: Ba19. Not save account if user requested wipe
	Given I have this account to create
			| Name         | Url  | Yellow | Red |
			| Account Ba19 | ba19 |        |     |
		But the user asked data wipe
	When I try to save the account
	Then I will receive this core error: UserAskedWipe
