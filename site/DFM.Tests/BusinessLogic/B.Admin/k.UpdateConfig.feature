Feature: k. Update Config

Background:
	Given I have an active user
	And I enable Categories use
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
	And I have this schedule to create
		| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Schedule Bk02 | 2014-03-04 | Out    | 10    | 10    | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: CategoriesDisabled
	And the schedule will not be saved

Scenario: 03. Disable categories use and create a category (E)
	Given I disable Categories use
	And I have this category to create
		| Name          |
		| Category Bk03 |
	When I try to save the category
	Then I will receive this core error: CategoriesDisabled
	And the category will not be saved

Scenario: 04. Disable categories use and select a category (E)
	Given I pass a valid category name
	And I disable Categories use
	When I try to get the category by its name
	Then I will receive this core error: CategoriesDisabled
	And I will receive no category

Scenario: 05. Disable categories use and disable a category (E)
	Given I give the enabled category Bk05
	And I disable Categories use
	When I try to disable the category
	Then I will receive this core error: CategoriesDisabled
	
Scenario: 06. Disable categories use and enable a category (E)
	Given I give the disabled category Bk06
	And I disable Categories use
	When I try to enable the category
	Then I will receive this core error: CategoriesDisabled



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
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: 92. Disable categories use and save schedule without category (S)
	Given I disable Categories use
	And I have this schedule to create
		| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Schedule Bk92 | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
	And the schedule will be saved
