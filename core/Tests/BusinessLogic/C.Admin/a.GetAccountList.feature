Feature: Ca. Get account list

Background:
	Given test user login
		And these settings
			| UseCategories |
			| false         |

Scenario: Ca01. Get all active accounts
	Given I have this account
			| Name           |
			| Account Bl01.1 |
		And I have this account
			| Name           |
			| Account Bl01.2 |
	When ask for the active account list
	Then I will receive no core error
		And the account list will have this
			| Name           | Url            |
			| Account Bl01.1 | account_bl01_1 |
			| Account Bl01.2 | account_bl01_2 |

Scenario: Ca02. Get all active accounts after close one
	Given I have this account
			| Name           |
			| Account Bl02.1 |
		And I have this account
			| Name           |
			| Account Bl02.2 |
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

Scenario: Ca03. Get all not active accounts after close one
	Given I have this account
			| Name           |
			| Account Bl03.1 |
		And I have this account
			| Name           |
			| Account Bl03.2 |
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

Scenario: Ca04. Not get accounts if user is marked for deletion
	Given I have this account
			| Name         |
			| Account Bl04 |
		But the user is marked for deletion
	When ask for the not active account list
	Then I will receive this core error: UserDeleted

Scenario: Ca05. Not get accounts if user requested wipe
	Given I have this account
			| Name         |
			| Account Bl05 |
		But the user asked data wipe
	When ask for the not active account list
	Then I will receive this core error: UserAskedWipe

Scenario: Ca06. Not get active accounts without signing contract
	Given there is a new contract
	When ask for the active account list
	Then I will receive this core error: NotSignedLastContract

Scenario: Ca07. Not get not active accounts without signing contract
	Given there is a new contract
	When ask for the not active account list
	Then I will receive this core error: NotSignedLastContract
