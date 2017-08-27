Feature: b. Run the schedules for an user

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category
	And I run the scheduler to cleanup older tests

Scenario: 01. Run with unlogged user (E)
	Given I have no logged user (logoff)
	When I try to run the scheduler
	Then I will receive this core error: Unauthorized


Scenario: 91. Run with bounded schedule (S)
	Given I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment | 
		| Move Db91   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -10

Scenario: 92. Run with boundless schedule (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db92   |      | Out    | 10    |       | True      | Monthly   | False           |
	And its Date is 3 months ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -40

Scenario: 93. Run schedule that will finish (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db93   |      | Out    | 10    | 3     | False     | Daily     | False           |
	And its Date is 5 days ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -30

Scenario: 94. Run schedule that wont finish (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db94   |      | Out    | 10    | 7     | False     | Daily     | False           |
	And its Date is 5 days ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -60

Scenario: 95. Run with daily schedule (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db95   |      | Out    | 10    | 10    | False     | Daily     | False           |
	And its Date is 20 days ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -100

Scenario: 96. Run with monthly schedule (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db96   |      | Out    | 10    | 6     | False     | Monthly   | False           |
	And its Date is 7 months ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -60

Scenario: 97. Run with yearly schedule (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db97   |      | Out    | 10    | 2     | False     | Monthly   | False           |
	And its Date is 2 years ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -20

Scenario: 98. Run with details in schedule (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db98   |      | Out    |       | 5     | False     | Daily     | False           |
	And its Date is 10 days ago
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -100



Scenario: 99. Run with e-mail system out (S)
	Given I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db99   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler with e-mail system out
	Then I will receive no core error
	And the accountOut value will change in -10

Scenario: 9A. Run with e-mail system ok (S)
	Given I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db9A   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler with e-mail system ok
	Then I will receive no core error
	And the accountOut value will change in -10



Scenario: 9B. Run with schedule start in past and end in future (S)
	Given I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db9B   |      | Out    | 10    | 5     | False     | Monthly   | False           |
	And its Date is 2 months ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to run the scheduler
	Then I will receive no core error
	And the accountOut value will change in -30