Feature: a. Creation of schedules

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Save without Description (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		|             | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveDescriptionRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 02. Save without Date (E)
	Given I have this future move to create
		| Description | Date | Nature | Value |
		| Move Da02   |      | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveDateRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 03. Save without Category (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da03   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: InvalidCategory
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 04. Save with unknown Category (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da04   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has an unknown Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: InvalidCategory
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 05. Save with Description too large (E)
	Given I have this future move to create
		| Description                                         | Date       | Nature | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: TooLargeData
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change



Scenario: 11. Save with (Nature: Out) (AccountOut:No) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da11   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: OutMoveWrong
	And the schedule will not be saved

Scenario: 12. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da12   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error: OutMoveWrong
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 13. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da13   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error: OutMoveWrong
	And the schedule will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 14. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da14   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an unknown Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: InvalidAccount
	And the schedule will not be saved



Scenario: 21. Save with (Nature: In) (AccountOut:No) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da21   | 2012-03-31 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: InMoveWrong
	And the schedule will not be saved

Scenario: 22. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da22   | 2012-03-31 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error: InMoveWrong
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 23. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da23   | 2012-03-31 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: InMoveWrong
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 24. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown) (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da24   | 2012-03-31 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this error: InvalidAccount
	And the schedule will not be saved



Scenario: 31. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da31   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: TransferMoveWrong
	And the schedule will not be saved

Scenario: 32. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da32   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error: TransferMoveWrong
	And the schedule will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 33. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da33   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: TransferMoveWrong
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 34. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown) (E)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da34   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this error: InvalidAccount
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 35. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes) (E)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da35   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an unknown Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error: InvalidAccount
	And the schedule will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 36. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out) (E)
Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da36   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In equal to Out
	When I try to save the schedule
	Then I will receive this error: MoveCircularTransfer
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change



Scenario: 41. Save without Value or Details (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da41   | 2012-03-31 | Out    |       |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveValueOrDetailRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 42. Save with Value zero and no Details (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da42   | 2012-03-31 | Out    | 0     |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveValueOrDetailRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 43. Save without value and without Description in Detail (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da43   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		|             | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveDetailDescriptionRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 44. Save without value and with Amount zero in Detail (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da44   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 0      | 10    |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveDetailAmountRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 45. Save without value and with Value zero in Detail (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da45   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 0     |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: MoveDetailValueRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 46. Save with Description too large in Detail (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da46   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description                                         | Amount | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: TooLargeData
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change



Scenario: 51. Save without Schedule (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da51   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has no schedule
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: ScheduleRequired
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 52. Save with Schedule Times zero and bounded (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da52   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 0     | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error: ScheduleTimesCantBeZero
	And the schedule will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change



Scenario: 91. Save with info all right (Out) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da91   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 92. Save with info all right (In) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da92   | 2012-03-31 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change
	
Scenario: 93. Save with info all right (Transfer) (S)
	Given I have this future move to create
		| Description | Date       | Nature   | Value |
		| Move Da93   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: 94. Save with info all right (value) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da94   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	
Scenario: 95. Save with info all right (details) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da95   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 96. Save negative (value) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da96   | 2012-03-31 | Out    | -10   |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 97. Save negative (details) (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da97   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | -10   |
		| Detail 2    | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 98. Save with future Date (E)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da98   | 2099-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 99. Save with exactly length in Description of Detail (S)
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Da99   | 2010-03-31 | Out    |       |
	And the move has this details
		| Description                                        | Amount | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1      | 10    |
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 9A. Save with exactly length in Description (S)
	Given I have this future move to create
		| Description                                        | Date       | Nature | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2010-03-31 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive no error
	And the schedule will be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change