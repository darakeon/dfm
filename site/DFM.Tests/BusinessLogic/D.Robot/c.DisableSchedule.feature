Feature: c. Disable schedule

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category

Scenario: 01. Disable a Schedule that doesn't exist (E)
	Given I pass an id of Schedule that doesn't exist
	When I try to disable the Schedule
	Then I will receive this core error: InvalidSchedule

Scenario: 02. Disable a Schedule already disabled (E)
	Given I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db91   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	And I already have disabled the Schedule
	When I try to disable the Schedule
	Then I will receive this core error: DisabledSchedule

Scenario: 99. Disable a Schedule with info all right (S)
	Given I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db91   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	When I try to disable the Schedule
	Then I will receive no core error
	And the Schedule will be disabled