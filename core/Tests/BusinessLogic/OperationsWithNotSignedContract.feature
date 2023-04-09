Feature: Operations with not signed contract

Background:
	Given test user login
		And I enable Categories use

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
