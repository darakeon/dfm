﻿Feature: Db. Run schedule

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Db01. Run with unlogged user
	Given I have no logged user (logoff)
	When run the scheduler
	Then I will receive this core error: Uninvited

Scenario: Db02. Run with bounded schedule
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db02   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -10
		And the schedule last run will be 1
		And the schedule will be disabled

Scenario: Db03. Run with boundless schedule
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db03   |      | Out    | 10    |       | True      | Monthly   | False           |
		And its Date is 3 months ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -40
		And the schedule last run will be 4
		And the schedule will be enabled

Scenario: Db04. Run schedule that will finish
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db04   |      | Out    | 10    | 3     | False     | Daily     | False           |
		And its Date is 5 days ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -30
		And the schedule last run will be 3
		And the schedule will be disabled

Scenario: Db05. Run schedule that wont finish
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db05   |      | Out    | 10    | 7     | False     | Daily     | False           |
		And its Date is 5 days ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -60
		And the schedule last run will be 6
		And the schedule will be enabled

Scenario: Db06. Run with daily schedule
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db06   |      | Out    | 10    | 10    | False     | Daily     | False           |
		And its Date is 20 days ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -100
		And the schedule last run will be 10
		And the schedule will be disabled

Scenario: Db07. Run with monthly schedule
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db07   |      | Out    | 10    | 6     | False     | Monthly   | False           |
		And its Date is 7 months ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -60
		And the schedule last run will be 6
		And the schedule will be disabled

Scenario: Db08. Run with yearly schedule
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db08   |      | Out    | 10    | 2     | False     | Monthly   | False           |
		And its Date is 2 years ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -20
		And the schedule last run will be 2
		And the schedule will be disabled

Scenario: Db09. Run with details in schedule
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db09   |      | Out    |       | 5     | False     | Daily     | False           |
		And its Date is 10 days ago
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 1      | 10    |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -100
		And the schedule last run will be 5
		And the schedule will be disabled

Scenario: Db10. Run with e-mail system out
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler with e-mail system out
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -10
		And the schedule last run will be 1
		And the schedule will be disabled

Scenario: Db11. Run with e-mail system ok
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db11   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler with e-mail system ok
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -10
		And the schedule last run will be 1
		And the schedule will be disabled

Scenario: Db12. Run with schedule start in past and end in future
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db12   |      | Out    | 10    | 5     | False     | Monthly   | False           |
		And its Date is 2 months ago
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -30
		And the schedule last run will be 3
		And the schedule will be enabled

Scenario: Db13. Run with bugged schedule
	Given I have a bugged schedule
	When robot user login
		And run the scheduler
	Then I will receive a core error

Scenario: Db14. Run schedule with category and categories use disabled
	Given I enable Categories use
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db14   | 2020-10-12 | Out    | 10    | 1     | False     | Daily     | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I disable Categories use
	When robot user login
		And run the scheduler
	Then I will receive this core error: CategoriesDisabled

Scenario: Db15. Run schedule without category and categories use enabled
	Given I disable Categories use
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db15   | 2020-10-12 | Out    | 10    | 1     | False     | Daily     | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I enable Categories use
	When robot user login
		And run the scheduler
	Then I will receive this core error: InvalidCategory

Scenario: Db16. Run with normal user
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db17   | 2021-04-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When run the scheduler
	Then I will receive this core error: Uninvited

Scenario: Db17. Run only that timezone schedules
	Given I have this user created
			| Email                       | Password | Active | Signed | Timezone |
			| tz_now@dontflymoney.com     | password | true   | true   | +0       |
			| tz_not_now@dontflymoney.com | password | true   | true   | +1       |
		And a schedule is created by tz_now@dontflymoney.com
		And a schedule is created by tz_not_now@dontflymoney.com
		But robot already ran for tz_not_now@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                   | Password |
			| tz_now@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email                       | Password |
			| tz_not_now@dontflymoney.com | password |
	Then the accountOut value will not change

Scenario: Db18. Run only active users
	Given I have this user created
			| Email                     | Password | Active | Signed |
			| active@dontflymoney.com   | password | true   | true   |
			| inactive@dontflymoney.com | password | true   | true   |
		And a schedule is created by active@dontflymoney.com
		And a schedule is created by inactive@dontflymoney.com
		And I deactivate the user inactive@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                   | Password |
			| active@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email                     | Password |
			| inactive@dontflymoney.com | password |
	Then the accountOut value will not change

Scenario: Db19. Do not run robots
	Given I have this user created
			| Email                   | Password | Active | Signed |
			| common@dontflymoney.com | password | true   | true   |
			| zb@dontflymoney.com     | password | true   | true   |
		And a schedule is created by common@dontflymoney.com
		And a schedule is created by zb@dontflymoney.com
		But zb@dontflymoney.com is a robot
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                   | Password |
			| common@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email               | Password |
			| zb@dontflymoney.com | password |
	Then the accountOut value will not change

Scenario: Db20. Run scheduler after add schedule
	Given I have this user created
			| Email                         | Password | Active | Signed |
			| new_schedule@dontflymoney.com | password | true   | true   |
		And a schedule is created by new_schedule@dontflymoney.com
		And robot run the scheduler
		And a schedule is created by new_schedule@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                         | Password |
			| new_schedule@dontflymoney.com | password |
	Then the accountOut value will change in -16
