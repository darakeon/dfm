Feature: Ia. Run schedule

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have two accounts
		And I have a category

Scenario: Ia01. Run with unlogged user
	Given I have no logged user (logoff)
	When run the scheduler
	Then I will receive this core error: Uninvited

Scenario: Ia02. Run with bounded schedule
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

Scenario: Ia03. Run with boundless schedule
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

Scenario: Ia04. Run schedule that will finish
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

Scenario: Ia05. Run schedule that wont finish
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

Scenario: Ia06. Run with daily schedule
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

Scenario: Ia07. Run with monthly schedule
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

Scenario: Ia08. Run with yearly schedule
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

Scenario: Ia09. Run with details in schedule
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

Scenario: Ia10. Run with e-mail system out
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And these settings
			| SendMoveEmail |
			| true          |
		But email system is out
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -10
		And the schedule last run will be 1
		And the schedule will be disabled

Scenario: Ia11. Run with e-mail system ok
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db11   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And these settings
			| SendMoveEmail |
			| true          |
	When robot user login
		And run the scheduler
	Then I will receive no core error
	Given test user login
		Then the accountOut value will change in -10
		And the schedule last run will be 1
		And the schedule will be disabled

Scenario: Ia12. Run with schedule start in past and end in future
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

Scenario: Ia13. Run with bugged schedule
	Given I have a bugged schedule
	When robot user login
		And run the scheduler
	Then I will receive a core error
		And the schedule status will be Error

Scenario: Ia14. Run schedule with category and categories use disabled
	Given these settings
			| UseCategories |
			| true          |
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db14   | 2020-10-12 | Out    | 10    | 1     | False     | Daily     | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And these settings
			| UseCategories |
			| false         |
	When robot user login
		And run the scheduler
	Then I will receive this core error: CategoriesDisabled
		And the schedule status will be CategoriesDisabled

Scenario: Ia15. Run schedule without category and categories use enabled
	Given these settings
			| UseCategories |
			| false         |
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db15   | 2020-10-12 | Out    | 10    | 1     | False     | Daily     | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And these settings
			| UseCategories |
			| true          |
	When robot user login
		And run the scheduler
	Then I will receive this core error: InvalidCategory
		And the schedule status will be CategoryInvalid

Scenario: Ia16. Run with normal user
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
		And the schedule status will be Ok

Scenario: Ia17. Run only that timezone schedules
	Given I have this user created
			| Email                                      | Password | Active | Signed | Timezone |
			| tz_now_{scenarioCode}@dontflymoney.com     | password | true   | true   | +0       |
			| tz_not_now_{scenarioCode}@dontflymoney.com | password | true   | true   | +1       |
		And a schedule is created by tz_now_{scenarioCode}@dontflymoney.com
		And a schedule is created by tz_not_now_{scenarioCode}@dontflymoney.com
		But robot already ran for tz_not_now_{scenarioCode}@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                                  | Password |
			| tz_now_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email                                      | Password |
			| tz_not_now_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will not change

Scenario: Ia18. Run only active users
	Given I have this user created
			| Email                                    | Password | Active | Signed |
			| active_{scenarioCode}@dontflymoney.com   | password | true   | true   |
			| inactive_{scenarioCode}@dontflymoney.com | password | true   | true   |
		And a schedule is created by active_{scenarioCode}@dontflymoney.com
		And a schedule is created by inactive_{scenarioCode}@dontflymoney.com
		And I deactivate the user inactive_{scenarioCode}@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                                  | Password |
			| active_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email                                    | Password |
			| inactive_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will not change
		And the status of last schedule of inactive_{scenarioCode} will be UserInactive

Scenario: Ia19. Do not run robots
	Given I have this user created
			| Email                                  | Password | Active | Signed |
			| common_{scenarioCode}@dontflymoney.com | password | true   | true   |
			| zb_{scenarioCode}@dontflymoney.com     | password | true   | true   |
		And a schedule is created by common_{scenarioCode}@dontflymoney.com
		And a schedule is created by zb_{scenarioCode}@dontflymoney.com
		But zb_{scenarioCode}@dontflymoney.com is a robot
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                                  | Password |
			| common_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will change in -8
	Given I logoff the user
		And I login this user
			| Email                              | Password |
			| zb_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will not change
		And the status of last schedule of zb_{scenarioCode} will be UserRobot

Scenario: Ia20. Run scheduler after add schedule
	Given I have this user created
			| Email                                        | Password | Active | Signed |
			| new_schedule_{scenarioCode}@dontflymoney.com | password | true   | true   |
		And a schedule is created by new_schedule_{scenarioCode}@dontflymoney.com
		And robot run the scheduler
		And a schedule is created by new_schedule_{scenarioCode}@dontflymoney.com
	When robot user login
		And run the scheduler
	Given I logoff the user
		And I login this user
			| Email                                        | Password |
			| new_schedule_{scenarioCode}@dontflymoney.com | password |
	Then the accountOut value will change in -16

Scenario: Ia21. Not run scheduler if user is marked for deletion
	Given I have this user created
			| Email                                   | Password | Active | Signed |
			| deleted_{scenarioCode}@dontflymoney.com | password | true   | true   |
		And a schedule is created by deleted_{scenarioCode}@dontflymoney.com
		But the user deleted_{scenarioCode}@dontflymoney.com is marked for deletion
	When robot user login
		And run the scheduler
	Then the user deleted_{scenarioCode}@dontflymoney.com will still have no moves
		And the status of last schedule of deleted_{scenarioCode} will be UserMarkedDelete

Scenario: Ia22. Not run scheduler if user has not signed last contract
	Given I have this user created
			| Email                                      | Password | Active | Signed |
			| not_signed_{scenarioCode}@dontflymoney.com | password | true   | true   |
		And a schedule is created by not_signed_{scenarioCode}@dontflymoney.com
		And I have a contract
	When robot user login
		And run the scheduler
	Then I will receive no core error
		But the user not_signed_{scenarioCode}@dontflymoney.com will still have no moves
		And the status of last schedule of not_signed_{scenarioCode} will be UserNoSignContract

Scenario: Ia23. Not run scheduler if user requested wipe
	Given I have this user created
			| Email                                     | Password | Active | Signed |
			| askedwipe_{scenarioCode}@dontflymoney.com | password | true   | true   |
		And a schedule is created by askedwipe_{scenarioCode}@dontflymoney.com
		But the user askedwipe_{scenarioCode}@dontflymoney.com asked data wipe
	When robot user login
		And run the scheduler
	Then the user askedwipe_{scenarioCode}@dontflymoney.com will still have no moves
		And the status of last schedule of askedwipe_{scenarioCode} will be UserRequestedWipe

Scenario: Ia24. Run account out + month above limits
	Given these limits in user plan
			| ScheduleActive | MoveByAccountByMonth |
			| 1              | 5                    |
		And I have moves of
			| Description           | Date       | Nature | Value | 
			| Move {scenarioCode} 1 | 2024-09-13 | Out    | 1     |
			| Move {scenarioCode} 2 | 2024-09-13 | Out    | 2     |
			| Move {scenarioCode} 3 | 2024-09-13 | Out    | 3     |
		And I have this schedule to create
			| Description         | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move {scenarioCode} | 2024-09-13 | Out    | 10    | 10    | False     | Daily     | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive this core error: PlanLimitMoveByAccountByMonthAchieved
		And the schedule status will be MoveOutOfLimit
	Given test user login
	Then the accountOut value will change in -20
		And the schedule last run will be 2
		And the schedule will be enabled

Scenario: Ia25. Run account in + month above limits
	Given these limits in user plan
			| ScheduleActive | MoveByAccountByMonth |
			| 1              | 5                    |
		And I have moves of
			| Description           | Date       | Nature | Value | 
			| Move {scenarioCode} 1 | 2024-09-13 | In     | 1     |
			| Move {scenarioCode} 2 | 2024-09-13 | In     | 2     |
			| Move {scenarioCode} 3 | 2024-09-13 | In     | 3     |
		And I have this schedule to create
			| Description         | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move {scenarioCode} | 2024-09-13 | In     | 10    | 10    | False     | Daily     | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
		And I save the schedule
	When robot user login
		And run the scheduler
	Then I will receive this core error: PlanLimitMoveByAccountByMonthAchieved
		And the schedule status will be MoveOutOfLimit
	Given test user login
	Then the accountIn value will change in 20
		And the schedule last run will be 2
		And the schedule will be enabled

Scenario: Ia26. Run after disable Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But I disable the category Category
	When robot user login
		And run the scheduler
	Then I will receive this core error: DisabledCategory
		And the schedule status will be CategoryDisabled
	Given test user login
		Then the accountOut value will not change

Scenario: Ia27. Run after close Account Out
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But I close the account Account Out
		And schedule is still enabled
	When robot user login
		And run the scheduler
	Then I will receive this core error: ClosedAccount
		And the schedule status will be AccountClosed
	Given test user login
		Then the accountOut value will not change

Scenario: Ia28. Run after close Account In
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | In     | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
		And I save the schedule
		But I close the account Account In
		And schedule is still enabled
	When robot user login
		And run the scheduler
	Then I will receive this core error: ClosedAccount
		And the schedule status will be AccountClosed
	Given test user login
		Then the accountIn value will not change

Scenario: Ia29. With conversion but change to same currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this schedule to create
			| Description | Date       | Nature   | Value | Conversion | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2024-11-02 | Transfer | 2     | 12         | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account In BRL
		And it has an Account Out EUR
		And I save the schedule
		But Account Out currency is set to BRL
	When robot user login
		And run the scheduler
	Then I will receive this core error: AccountsSameCurrencyConversion
		And the schedule status will be CurrencyChange
	Given test user login
		Then the accountIn value will not change

Scenario: Ia30. Without conversion but change to different currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2024-11-02 | Transfer | 2     | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account In EUR
		And it has an Account Out EUR
		And I save the schedule
		But Account In currency is set to BRL
	When robot user login
		And run the scheduler
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the schedule status will be CurrencyChange
	Given test user login
		Then the accountIn value will not change

Scenario: Ia31. Recover from failure
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Db10   | 2014-03-22 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		But I disable the category Category
		And robot run the scheduler
		And I enable the category Category
	When robot user login
		And run the scheduler
	Then I will receive no core error
		And the schedule status will be Ok
