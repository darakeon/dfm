Feature: Hb. HasSchedules

Background:
	Given test user login
		And I have two accounts

Scenario: Hb01. No logged in user
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df01   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		But I have no logged user (logoff)
	When ask if the user has Schedules
	Then I will receive this core error: Uninvited

Scenario: Hb02. No active in user
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df02   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		But the user not active after 15 days
	When ask if the user has Schedules
	Then I will receive this core error: DisabledUser

Scenario: Hb03. No schedules
	When ask if the user has Schedules
	Then the answer is No

Scenario: Hb04. With schedule
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df04   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When ask if the user has Schedules
	Then the answer is Yes

Scenario: Hb05. After schedule finishes
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df05   | 2022-06-25 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And robot run the scheduler
	When ask if the user has Schedules
	Then the answer is No

Scenario: Hb06. After schedule is disabled
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df06   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I disable the schedule
	When ask if the user has Schedules
	Then the answer is No

Scenario: Hb07. Not save if user is marked for deletion
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df07   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But the user is marked for deletion
	When ask if the user has Schedules
	Then I will receive this core error: UserDeleted

Scenario: Hb08. Not save if user requested wipe
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df08   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But the user asked data wipe
	When ask if the user has Schedules
	Then I will receive this core error: UserAskedWipe

Scenario: Hb09. Not save if user didn't signed last contract
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Df09   | 2022-06-25 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But there is a new contract
	When ask if the user has Schedules
	Then I will receive this core error: NotSignedLastContract
