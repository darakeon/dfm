Feature: Aq. Operations with not signed Contract

Background: 
	Given I have an active user

Scenario: Aq001. List Logins
	Given I have an active user
	And I login the user
	And I logoff the user
	And I login the user
	But there is a new contract
	When I ask for current active logins
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq002. Change Password
	Given I pass this password
		| Current Password | Password     | Retype Password |
		| password         | new_password | new_password    |
	But there is a new contract
	When I try to change the password
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq003. Update E-mail
	Given I pass this new e-mail and password
		| New E-mail              | Current Password |
		| Aq003_@dontflymoney.com | password         |
	But there is a new contract
	When I try to change the e-mail
	Then I will receive this core error: NotSignedLastContract


Scenario: Aq101. Save Account
	Given I have this account to create
		| Name          | Url           | Yellow | Red |
		| Account Aq101 | account_aq101 |        |     |
	But there is a new contract
	When I try to save the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq102. Select Account
	Given I have an account
	And I pass a valid account url
	But there is a new contract
	When I try to get the account by its url
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq103. Update Account
	Given I have this account
		| Name          | Url           | Yellow | Red |
		| Account Aq103 | account_aq103 |        |     |
	But there is a new contract
	When I make this changes to the account
		| Name             | Url           | Yellow | Red |
		| Aq103 - new name | account_aq103 |        |     |
	And I try to update the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq104. Close Account
	Given I have a category
	And I give a url of the account Aq104 with moves
	But there is a new contract
	When I try to close the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq105. Delete Account
	Given I give a url of the account Aq105 without moves
	But there is a new contract
	When I try to delete the account
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq106. Save Category
	Given I have this category to create
		| Name           |
		| Category Aq106 |
	But there is a new contract
	When I try to save the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq107. Select Category
	Given I have a category
	And I pass a valid category name
	But there is a new contract
	When I try to get the category by its name
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq108. Update Category
	Given I have this category
		| Name           |
		| Category Aq108 |
	And I make this changes to the category
		| Name             |
		| Aq108 - new name |
	But there is a new contract
	When I try to update the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq109. Disable Category
	Given I give the enabled category Aq109
	But there is a new contract
	When I try to disable the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq110. Enable Category
	Given I give the disabled category Aq110
	But there is a new contract
	When I try to enable the category
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq111. Update Config - Categories Disable
	Given there is a new contract
	When I try to disable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq112. Update Config - Categories Enable
	Given there is a new contract
	When I try to enable Categories use
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq113. Update Config - Move Send E-mail Disable
	Given there is a new contract
	When I try to disable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq114. Update Config - Move Send E-mail Enable
	Given there is a new contract
	When I try to enable move send e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq115. Update Config - Change Language
	Given there is a new contract
	When I try to change the language to en-US
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq116. Update Config - Change TimeZone
	Given there is a new contract
	When I try to change the timezone to E. South America Standard Time
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq117. Get active Account list
	Given there is a new contract
	When ask for the active account list
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq118. Get not active Account list
	Given there is a new contract
	When ask for the not active account list
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq119. Get Category list
	Given there is a new contract
	When ask for the active category list
	Then I will receive this core error: NotSignedLastContract


Scenario: Aq201. Save Move
	Given I have a category
	And I have two accounts
	And I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca94   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	But there is a new contract
	When I try to save the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq202. Update Move
	Given I have a category
	And I have two accounts
	And I have a move with value 10 (Out)
	And I change the move date in -1 day
	But there is a new contract
	When I update the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq203. Select Move
	Given I enable Categories use
	And I have two accounts
	And I have a category
	And I have a move
	And I pass valid Move ID
	But there is a new contract
	When I try to get the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq204. Select Detail
	Given I enable Categories use
	And I have two accounts
	And I have a category
	And I have a move with details
	And I pass valid Detail ID
	But there is a new contract
	When I try to get the detail
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq205. Delete Move
	Given I have a category
	And I have two accounts
	And I have a move with value 10 (In)
	And I pass valid Move ID
	But there is a new contract
	When I try to delete the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq206. Check Move
	Given I have a category
		And I have two accounts
		And I enable move check
		And I have a move with value 10 (Out)
		And the move is not checked
		But there is a new contract
	When I try to mark it as checked
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq207. Uncheck Move
	Given I have a category
		And I have two accounts
		And I enable move check
		And I have a move with value 10 (Out)
		And the move is checked
		But there is a new contract
	When I try to mark it as not checked
	Then I will receive this core error: NotSignedLastContract


Scenario: Aq301. Save Schedule
	Given I have a category
	And I have two accounts
	And I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Da91   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	But there is a new contract
	When I try to save the schedule
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq302. Run Schedule
	Given I have a category
	And I have two accounts
	And I have this schedule to create
		| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db94   |      | Out    | 10    | 7     | False     | Daily     | False           |
	And its Date is 5 days ago
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	But there is a new contract
	When I try to run the scheduler
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq303. Disable Schedule
	Given I enable Categories use
	And I have two accounts
	And I have a category
	And I have this schedule to create
		| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
		| Move Db91   | 2012-03-31 | Out    | 10    | 1     | False     | Monthly   | False           |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	And I save the schedule
	But there is a new contract
	When I try to disable the Schedule
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq304. Get Schedule List
	Given there is a new contract
	When ask for the schedule list
	Then I will receive this core error: NotSignedLastContract


Scenario: Aq401. Get Month Report
	Given there is a new contract
	When I try to get the month report
	Then I will receive this core error: NotSignedLastContract

Scenario: Aq402. Get Year Report
	Given there is a new contract
	When I try to get the year report
	Then I will receive this core error: NotSignedLastContract

