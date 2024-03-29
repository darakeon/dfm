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
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba02 | 100    | 200 |
	When I try to save the account
	Then I will receive this core error: RedLimitAboveYellowLimit
		And the account will not be saved

Scenario: Cb03. Save Account with name that already exists
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba03 |        |     |
		And I already have this account
			| Name         | Yellow | Red |
			| Account Ba03 | 200    | 100 |
	When I try to save the account
	Then I will receive this core error: AccountNameAlreadyExists
		And the account will not be changed

Scenario: Cb04. Save Account with too big name
	Given I have this account to create
			| Name                  | Yellow | Red |
			| ABCDEFGHIJKLMNOPQRSTU |        |     |
	When I try to save the account
	Then I will receive this core error: TooLargeAccountName
		And the account will not be saved

Scenario: Cb05. Save Account with exactly length name
	Given I have this account to create
			| Name                 | Yellow | Red |
			| ABCDEFGHIJKLMNOPQRST |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb06. Save Account with special character
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account/Ba06 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb07. Save Account with with space
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba07 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb08. Save Account that already exists equal url
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba08 |        |     |
		And I already have this account
			| Name         | Yellow | Red |
			| Account_Ba08 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account url will be account_ba08_2

Scenario: Cb09. Save Account with info all right (without limits)
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba09 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb10. Save Account with just yellow limit
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba10 | 100    |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb11. Save Account with just red limit
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba11 |        | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb12. Save Account with info all right (with limits)
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba12 | 200    | 100 |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
		And the account will be open

Scenario: Cb13. Save Account with same name in another user
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba13 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved
	Given there is another person logged in
		And I have this account to create
			| Name         | Yellow | Red |
			| Account Ba13 |        |     |
	When I try to save the account
	Then I will receive no core error
		And the account will be saved

Scenario: Cb14. Not save account if user is marked for deletion
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba14 |        |     |
		But the user is marked for deletion
	When I try to save the account
	Then I will receive this core error: UserDeleted

Scenario: Cb15. Not save account if user requested wipe
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba15 |        |     |
		But the user asked data wipe
	When I try to save the account
	Then I will receive this core error: UserAskedWipe

Scenario: Cb16. Not create account without signing contract
	Given I have this account to create
			| Name         | Yellow | Red |
			| Account Ba16 |        |     |
		But there is a new contract
	When I try to save the account
	Then I will receive this core error: NotSignedLastContract
