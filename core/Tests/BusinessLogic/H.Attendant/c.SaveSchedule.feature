﻿Feature: Hc. Save schedule

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have two accounts
		And I have a category

Scenario: Hc01. Save without Description
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			|             | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveDescriptionRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc02. Save without Date
	Given I have this schedule to create
			| Description | Date | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da02   |      | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveDateRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc03. Save without Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da03   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidCategory
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc04. Save with unknown Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da04   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has an unknown Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidCategory
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc05. Save with Description too large
	Given I have this schedule to create
			| Description                                         | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: TooLargeScheduleDescription
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc06. Save with (Nature: Out) (AccountOut:No) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da06   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: OutMoveWrong
		And the schedule will not be saved

Scenario: Hc07. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da07   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive this core error: OutMoveWrong
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc08. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da08   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive this core error: OutMoveWrong
		And the schedule will not be saved
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc09. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da09   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an unknown Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidAccount
		And the schedule will not be saved

Scenario: Hc10. Save with (Nature: In) (AccountOut:No) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da10   | 2012-03-31 | In     | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: InMoveWrong
		And the schedule will not be saved

Scenario: Hc11. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da11   | 2012-03-31 | In     | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive this core error: InMoveWrong
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc12. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da12   | 2012-03-31 | In     | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: InMoveWrong
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc13. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da13   | 2012-03-31 | In     | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidAccount
		And the schedule will not be saved

Scenario: Hc14. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da14   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: TransferMoveWrong
		And the schedule will not be saved

Scenario: Hc15. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da15   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive this core error: TransferMoveWrong
		And the schedule will not be saved
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc16. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da16   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: TransferMoveWrong
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc17. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da17   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidAccount
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc18. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da18   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an unknown Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive this core error: InvalidAccount
		And the schedule will not be saved
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc19. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da19   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In equal to Out
	When I try to save the schedule
	Then I will receive this core error: CircularTransfer
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc20. Save without Value or Details
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da20   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveValueOrDetailRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc21. Save with Value zero and no Details
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da21   | 2012-03-31 | Out    | 0     | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveValueOrDetailRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc22. Save without value and without Description in Detail
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da22   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			|             | 1      | 10    |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveDetailDescriptionRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc23. Save without value and with Amount zero in Detail
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da23   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 0      | 10    |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveDetailAmountRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc24. Save without value and with Value zero in Detail
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da24   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 1      | 0     |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: MoveDetailValueRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc25. Save with Description too large in Detail
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da25   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description                                         | Amount | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: TooLargeDetailDescription
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc26. Save without Schedule
	Given I have no schedule
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: ScheduleRequired
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc27. Save with Schedule Times zero and bounded
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da27   | 2012-03-31 | Out    | 10    | 0     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: ScheduleTimesCantBeZero
		And the schedule will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc28. Save with info all right (Out)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da28   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc29. Save with info all right (In)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da29   | 2012-03-31 | In     | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc30. Save with info all right (Transfer)
	Given I have this schedule to create
			| Description | Date       | Nature   | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da30   | 2012-03-31 | Transfer | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Hc31. Save with info all right (value)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da31   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the next robot schedule run will check the user

Scenario: Hc32. Save with info all right (details)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da32   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 1      | 10    |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 20
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc33. Save negative (value)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da33   | 2012-03-31 | Out    | -10   | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc34. Save negative (details)
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da34   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 1      | -10   |
			| Detail 2    | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 20
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc35. Save with future Date
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da35   | 2099-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc36. Save with exactly length in Description of Detail
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da36   | 2010-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description                                        | Amount | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc37. Save with exactly length in Description
	Given I have this schedule to create
			| Description                                        | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2010-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc38. Save with details with same description
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da38   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Move Da38   | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
		And the schedule value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Hc39. Not save if user is marked for deletion
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da39   | 2021-05-16 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But the user is marked for deletion
	When I try to save the schedule
	Then I will receive this core error: UserDeleted

Scenario: Hc40. Not save if user requested wipe
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da40   | 2021-06-26 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But the user asked data wipe
	When I try to save the schedule
	Then I will receive this core error: UserAskedWipe

Scenario: Hc41. Not save if not signed last contract
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da41   | 2023-04-09 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But there is a new contract
	When I try to save the schedule
	Then I will receive this core error: NotSignedLastContract

Scenario: Hc42. Save with details above limits
	Given these limits in user plan
			| ScheduleActive | MoveDetail |
			| 1              | 3          |
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da25   | 2012-03-31 | Out    |       | 10    | False     | Monthly   | False           |
		And the schedule has this details
			| Description | Amount | Value |
			| Detail 1    | 1      | 10    |
			| Detail 2    | 2      | 20    |
			| Detail 3    | 3      | 30    |
			| Detail 4    | 4      | 40    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: PlanLimitMoveDetailAchieved
		And the schedule will not be saved

Scenario: Hc43. Save above limits
	Given these limits in user plan
			| ScheduleActive |
			| 1              |
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da28   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da28   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive this core error: PlanLimitScheduleActiveAchieved
		And the schedule will not be saved

Scenario: Hc44. Save but deactivate to reset limit
	Given these limits in user plan
			| ScheduleActive |
			| 1              |
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da28   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I disable the schedule
		And I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Da28   | 2012-03-31 | Out    | 10    | 10    | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the schedule
	Then I will receive no core error
		And the schedule will be saved
