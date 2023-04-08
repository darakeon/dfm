Feature: Operations with not signed contract

Background:
	Given test user login
		And I enable Categories use

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
