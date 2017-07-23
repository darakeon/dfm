Feature: b. Run the schedules for an user

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Run with unlogged user (E)
	Given I have no logged user (logoff)
	When I try to run the scheduler
	Then I will receive this error: Unauthorized
	And the user amount money will be kept



Scenario: 91. Run with bounded schedule (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 1     | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 10
	And the year-category-accountOut value will decrease in 10

Scenario: 92. Run with boundless schedule (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		|       | True      | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 10 plus the months until now
	And the year-category-accountOut value will decrease in 10 plus the months until now


Scenario: 93. Run schedule that will finish (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    | 10    |
	And its Date is 5 days ago
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 3     | False     | Daily     | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 30
	And the year-category-accountOut value will decrease in 30

Scenario: 94. Run schedule that wont finish (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    | 10    |
	And its Date is 5 days ago
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 7     | False     | Daily     | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 5
	And the year-category-accountOut value will decrease in 5


Scenario: 95. Run with daily schedule (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    | 10    |
	And its Date is 20 days ago
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Daily     | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 100
	And the year-category-accountOut value will decrease in 100

Scenario: 96. Run with monthly schedule (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    | 10    |
	And its Date is 190 days ago
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 6     | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 60
	And the year-category-accountOut value will decrease in 60

Scenario: 97. Run with yearly schedule (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    | 10    |
	And its Date is 750 days ago
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 2     | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 20
	And the year-category-accountOut value will decrease in 20


Scenario: 98. Run with details in schedule (S)
	Given I have this move to create
		| Description | Nature | Value |
		| Move Ca91   | Out    |       |
	And its Date is 10 days ago
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 5     | False     | Daily     | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the move
	When I run the scheduler
	Then the month-category-accountOut value will decrease in 100
	And the year-category-accountOut value will decrease in 100
	