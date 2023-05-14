Feature: Dd. Get schedule list

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Dd01. Get all schedules
	Given I have this schedule to create
			| Description     | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Dd01.1 | 2017-02-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I have this schedule to create
			| Description     | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Dd01.2 | 2017-02-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When ask for the schedule list
	Then I will receive no core error
		And the schedule list will have this
			| Name            |
			| Schedule Dd01.1 |
			| Schedule Dd01.2 |

Scenario: Dd02. Get all schedules after delete one
	Given I have this schedule to create
			| Description     | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Dd02.1 | 2017-02-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I have this schedule to create
			| Description     | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Dd02.2 | 2017-02-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I disable the schedule
	When ask for the schedule list
	Then I will receive no core error
		And the schedule list will have this
			| Name            |
			| Schedule Dd02.1 |
		And the schedule list will not have this
			| Name            |
			| Schedule Dd02.2 |

Scenario: Dd03. Not get schedules if user is marked for deletion
	Given the user is marked for deletion
	When ask for the schedule list
	Then I will receive this core error: UserDeleted

Scenario: Dd04. Not get schedules if user requested wipe
	Given the user asked data wipe
	When ask for the schedule list
	Then I will receive this core error: UserAskedWipe

Scenario: Dd05. Not get schedules if not signed last contract
	Given there is a new contract
	When ask for the schedule list
	Then I will receive this core error: NotSignedLastContract
