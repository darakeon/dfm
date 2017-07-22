Feature: a. Creation of schedules

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Save without Description (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		|             | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                   |
		| MoveDescriptionRequired |
	And the schedule will not be saved

Scenario: 02. Save without Date (E)
	Given I have this move to create
		| Description | Date | Nature | Value |
		| Move Ca02   |      | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error            |
		| MoveDateRequired |
	And the schedule will not be saved

Scenario: 03. Save without Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca04   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                |
		| MoveCategoryRequired |
	And the schedule will not be saved

Scenario: 04. Save with unknown Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca05   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has an unknown Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error               |
		| MoveCategoryInvalid |
	And the schedule will not be saved



Scenario: 11. Save with (Nature: Out) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca11   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the schedule will not be saved

Scenario: 12. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca12   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the schedule will not be saved

Scenario: 13. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca13   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the schedule will not be saved

Scenario: 14. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca14   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an unknown Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the schedule will not be saved



Scenario: 21. Save with (Nature: In) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca21   | 31/03/2012 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the schedule will not be saved

Scenario: 22. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca22   | 31/03/2012 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the schedule will not be saved

Scenario: 23. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca23   | 31/03/2012 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the schedule will not be saved

Scenario: 24. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca24   | 31/03/2012 | In     | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the schedule will not be saved



Scenario: 31. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca31   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the schedule will not be saved

Scenario: 32. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca32   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the schedule will not be saved

Scenario: 33. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca33   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the schedule will not be saved

Scenario: 34. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca34   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an unknown Account In
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the schedule will not be saved

Scenario: 35. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an unknown Account Out
	And it has an Account In
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the schedule will not be saved

Scenario: 36. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has an Account In equal to Out
	When I try to save the schedule
	Then I will receive this error
		| Error             |
		| MoveCircularTransfer |
	And the schedule will not be saved



Scenario: 41. Save without Value or Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca41   | 31/03/2012 | Out    |       |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                     |
		| MoveValueOrDetailRequired |
	And the schedule will not be saved

Scenario: 42. Save with Value zero and no Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca42   | 31/03/2012 | Out    | 0     |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 10    | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                     |
		| MoveValueOrDetailRequired |
	And the schedule will not be saved

Scenario: 43. Save without value and without Description in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca43   | 31/03/2012 | Out    |       |
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
	Then I will receive this error
		| Error                         |
		| MoveDetailDescriptionRequired |
	And the schedule will not be saved

Scenario: 44. Save without value and with Amount zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca44   | 31/03/2012 | Out    |       |
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
	Then I will receive this error
		| Error                    |
		| MoveDetailAmountRequired |
	And the schedule will not be saved

Scenario: 45. Save without value and with Value zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca45   | 31/03/2012 | Out    |       |
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
	Then I will receive this error
		| Error                   |
		| MoveDetailValueRequired |
	And the schedule will not be saved



Scenario: 51. Save without Schedule (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has no schedule
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                |
		| MoveScheduleRequired |
	And the schedule will not be saved

Scenario: 52. Save with Schedule Times zero and bounded (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 0     | False     | Monthly   | False           |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the schedule
	Then I will receive this error
		| Error                   |
		| ScheduleTimesCantBeZero |
	And the schedule will not be saved



Scenario: 91. Save with info all right (Out) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 31/03/2012 | Out    | 10    |
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
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	
Scenario: 92. Save with info all right (In) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca92   | 31/03/2012 | In     | 10    |
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
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change
	
Scenario: 93. Save with info all right (Transfer) (S)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca93   | 31/03/2012 | Transfer | 10    |
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
	And the month-category-accountOut value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountOut value will not change
	And the year-category-accountIn value will not change

Scenario: 94. Save with info all right (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 31/03/2012 | Out    | 10    |
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
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	
Scenario: 95. Save with info all right (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca94   | 31/03/2012 | Out    |       |
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
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 96. Save negative (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca95   | 31/03/2012 | Out    | -10   |
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
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 97. Save negative (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca96   | 31/03/2012 | Out    |       |
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
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
