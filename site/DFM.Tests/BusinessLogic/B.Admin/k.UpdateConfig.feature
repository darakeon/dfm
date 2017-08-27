Feature: k. Update Config

Background:
	Given I have an active user
	And I have a category

Scenario: 01. Disable categories use and save move with category (E)
	Given I disable Categories use
	And I have this move to create
		| Description | Date       | Nature | Value |
		| Move Bk01   | 2014-03-04 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: CategoriesDisabled
	And the move will not be saved

Scenario: 02. Disable categories use and save schedule with category (E)
	Given I disable Categories use
	Given I have this future move to create
		| Description   | Date       | Nature | Value |
		| Schedule Bk02 | 2014-03-04 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: CategoriesDisabled
	And the schedule will not be saved

Scenario: 91. Disable categories use and save move without category (S)
	Given I disable Categories use
	And I have this move to create
		| Description | Date       | Nature | Value |
		| Move Bk91   | 2014-03-04 | Out    | 10    |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved

Scenario: 92. Disable categories use and save schedule without category (S)
	Given I disable Categories use
	Given I have this future move to create
		| Description   | Date       | Nature | Value |
		| Schedule Bk92 | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
	And the schedule will be saved
