Feature: Dc. Disable schedule

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Dc01. Disable a Schedule that doesn't exist
	Given I pass an id of Schedule that doesn't exist
	When I try to disable the Schedule
	Then I will receive this core error: InvalidSchedule

Scenario: Dc02. Disable a Schedule already disabled
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc02   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I already have disabled the Schedule
	When I try to disable the Schedule
	Then I will receive this core error: DisabledSchedule

Scenario: Dc03. Disable a Schedule with info all right
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc03   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When I try to disable the Schedule
	Then I will receive no core error
		And the schedule will be disabled

Scenario: Dc04. Disable another user's Schedule
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc04   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But there is a bad person logged in
	When I try to disable the Schedule
	Then I will receive this core error: InvalidSchedule
	Given test user login
	Then the schedule will be enabled

Scenario: Dc05. Disable a Schedule with details
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc05   | 2012-03-31 | Out    |       | 1     | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail      | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When I try to disable the Schedule
	Then I will receive no core error
		And the schedule will be disabled

Scenario: Dc06. Not disable a Schedule if user is marked for deletion
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc06   | 2012-03-31 | Out    |       | 1     | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail      | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But the user is marked for deletion
	When I try to disable the Schedule
	Then I will receive this core error: UserDeleted

Scenario: Dc07. Not disable a Schedule if user requested wipe
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc07   | 2012-03-31 | Out    |       | 1     | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail      | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But the user asked data wipe
	When I try to disable the Schedule
	Then I will receive this core error: UserAskedWipe

Scenario: Dc08. Not disable if not signed last contract
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Dc08   | 2023-04-09 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But there is a new contract
	When I try to disable the Schedule
	Then I will receive this core error: NotSignedLastContract
