Feature: Operations with not signed contract

Background:
	Given test user login
		And I enable Categories use

Scenario: Zz04. Save Account
	Given I have this account to create
			| Name          | Yellow | Red |
			| Account Aq101 |        |     |
		But there is a new contract
	When I try to save the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz05. Select Account
	Given I have an account
		And I pass a valid account url
		But there is a new contract
	When I try to get the account by its url
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz06. Update Account
	Given I have this account
			| Name          | Yellow | Red |
			| Account Aq103 |        |     |
		But there is a new contract
	When I make this changes to the account
			| Name             | Yellow | Red |
			| Aq103 - new name |        |     |
		And I try to update the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz07. Close Account
	Given I have a category
		And I give a url of the account Aq104 with moves
		But there is a new contract
	When I try to close the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz08. Delete Account
	Given I give a url of the account Aq105 without moves
		But there is a new contract
	When I try to delete the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz09. Save Category
	Given I have this category to create
			| Name           |
			| Category Aq106 |
		But there is a new contract
	When I try to save the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz10. Select Category
	Given I have a category
		And I pass a valid category name
		But there is a new contract
	When I try to get the category by its name
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz11. Update Category
	Given I have this category
			| Name           |
			| Category Aq108 |
		And I make this changes to the category
			| Name             |
			| Aq108 - new name |
		But there is a new contract
	When I try to update the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz12. Disable Category
	Given I give the enabled category Aq109
		But there is a new contract
	When I try to disable the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz13. Enable Category
	Given I give the disabled category Aq110
		But there is a new contract
	When I try to enable the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz14. Update Settings - Categories Disable
	Given there is a new contract
	When I try to disable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz15. Update Settings - Categories Enable
	Given there is a new contract
	When I try to enable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz16. Update Settings - Move Send E-mail Disable
	Given there is a new contract
	When I try to disable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz17. Update Settings - Move Send E-mail Enable
	Given there is a new contract
	When I try to enable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz18. Update Settings - Change Language
	Given there is a new contract
	When I try to change the language to en-US
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz19. Update Settings - Change TimeZone
	Given there is a new contract
	When I try to change the timezone to UTC-03:00
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz20. Get active Account list
	Given there is a new contract
	When ask for the active account list
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz21. Get not active Account list
	Given there is a new contract
	When ask for the not active account list
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz22. Get Category list
	Given there is a new contract
	When ask for the active category list
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz23. Save Move
	Given I have a category
		And I have two accounts
		And I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca94   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But there is a new contract
	When I try to save the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz24. Update Move
	Given I have a category
		And I have two accounts
		And I have a move with value 10 (Out)
		And I change the move date in -1 day
		But there is a new contract
	When I update the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz25. Select Move
	Given I enable Categories use
		And I have two accounts
		And I have a category
		And I have a move
		And I pass valid Move ID
		But there is a new contract
	When I try to get the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz27. Delete Move
	Given I have a category
		And I have two accounts
		And I have a move with value 10 (In)
		And I pass valid Move ID
		But there is a new contract
	When I try to delete the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz28. Check Move
	Given I have a category
		And I have two accounts
		And I enable move check
		And I have a move with value 10 (Out)
		And the move is checked for account Out
		But there is a new contract
	When I try to mark it as checked for account Out
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz29. Uncheck Move
	Given I have a category
		And I have two accounts
		And I enable move check
		And I have a move with value 10 (Out)
		And the move is checked for account Out
		But there is a new contract
	When I try to mark it as not checked for account Out
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz30. Save Schedule
	Given I have a category
		And I have two accounts
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da91   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But there is a new contract
	When I try to save the schedule
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz32. Disable Schedule
	Given I enable Categories use
		And I have two accounts
		And I have a category
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db91   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But there is a new contract
	When I try to disable the Schedule
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz33. Get Schedule List
	Given there is a new contract
	When ask for the schedule list
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz34. Get Month Report
	Given there is a new contract
	When I try to get the month report
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz35. Get Year Report
	Given there is a new contract
	When I try to get the year report
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz36. Search by Description
	Given there is a new contract
	When I try to search by description Something
	Then I will receive this core error: NotSignedLastContract
