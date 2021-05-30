﻿Feature: Bl. Get account list

Background:
	Given test user login
		And I disable Categories use

Scenario: Bl01. Get all active accounts
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl01.1 | account_bl01_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl01.2 | account_bl01_2 |        |     |
	When ask for the active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            | Yellow | Red |
			| Account Bl01.1 | account_bl01_1 |        |     |
			| Account Bl01.2 | account_bl01_2 |        |     |

Scenario: Bl02. Get all active accounts after close one
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl02.1 | account_bl02_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl02.2 | account_bl02_2 |        |     |
		And this account has moves
		And I close the account account_bl02_2
	When ask for the active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            |
			| Account Bl02.1 | account_bl02_1 |
		And the account list will not have this
			| Name           | Url            |
			| Account Bl02.2 | account_bl02_2 |

Scenario: Bl03. Get all not active accounts after close one
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl03.1 | account_bl03_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Bl03.2 | account_bl03_2 |        |     |
		And this account has moves
		And I close the account account_bl03_2
	When ask for the not active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            |
			| Account Bl03.2 | account_bl03_2 |
		And the account list will not have this
			| Name           | Url            |
			| Account Bl03.1 | account_bl03_1 |
