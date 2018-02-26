Feature: Bl. Get Account List

Background:
	Given I have an active user
		And the user have accepted the contract

Scenario: Bl01. Get all active accounts
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al01.1 | account_al01_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al01.2 | account_al01_2 |        |     |
	When ask for the active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            | Yellow | Red |
			| Account Al01.1 | account_al01_1 |        |     |
			| Account Al01.2 | account_al01_2 |        |     |

Scenario: Bl02. Get all active accounts after close one
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al02.1 | account_al02_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al02.2 | account_al02_2 |        |     |
		And this account has moves
		And I close the account account_al02_2
	When ask for the active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            |
			| Account Al02.1 | account_al02_1 |
		And the account list will not have this
			| Name           | Url            |
			| Account Al02.2 | account_al02_2 |

Scenario: Bl03. Get all not active accounts after close one
	Given I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al03.1 | account_al03_1 |        |     |
		And I have this account
			| Name           | Url            | Yellow | Red |
			| Account Al03.2 | account_al03_2 |        |     |
		And this account has moves
		And I close the account account_al03_2
	When ask for the not active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            |
			| Account Al03.2 | account_al03_2 |
		And the account list will not have this
			| Name           | Url            |
			| Account Al03.1 | account_al03_1 |
