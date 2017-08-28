Feature: Ca. Creation of Move

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category

Scenario: Ca01. Save without Description (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		|             | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDescriptionRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca02. Save without Date (E)
	Given I have this move to create
		| Description | Date | Nature | Value |
		| Move Ca02   |      | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDateRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca03. Save with future Date (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca03   | 2099-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDateInvalid
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca04. Save without Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca04   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: InvalidCategory
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca05. Save with unknown Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca05   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has an unknown Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: InvalidCategory
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca06. Save with Description too large (E)
	Given I have this move to create
		| Description                                         | Date       | Nature | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: TooLargeData
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change



Scenario: Ca11. Save with (Nature: Out) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca11   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: OutMoveWrong
	And the move will not be saved

Scenario: Ca12. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca12   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this core error: OutMoveWrong
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: Ca13. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca13   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this core error: OutMoveWrong
	And the move will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: Ca14. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca14   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
	And the move will not be saved



Scenario: Ca21. Save with (Nature: In) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca21   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: InMoveWrong
	And the move will not be saved

Scenario: Ca22. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca22   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this core error: InMoveWrong
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: Ca23. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca23   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: InMoveWrong
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca24. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca24   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
	And the move will not be saved



Scenario: Ca31. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca31   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: TransferMoveWrong
	And the move will not be saved

Scenario: Ca32. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca32   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this core error: TransferMoveWrong
	And the move will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: Ca33. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca33   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: TransferMoveWrong
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca34. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca34   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca35. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
	And the move will not be saved
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change

Scenario: Ca36. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca36   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In equal to Out
	When I try to save the move
	Then I will receive this core error: MoveCircularTransfer
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change
	And the accountIn value will not change
	And the month-category-accountIn value will not change
	And the year-category-accountIn value will not change



Scenario: Ca41. Save without Value or Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca41   | 2012-03-31 | Out    |       |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveValueOrDetailRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca42. Save with Value zero and no Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca42   | 2012-03-31 | Out    | 0     |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveValueOrDetailRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca43. Save without value and without Description in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca43   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		|             | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDetailDescriptionRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca44. Save without value and with Amount zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca44   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 0      | 10    |
		| Detail 2    | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDetailAmountRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca45. Save without value and with Value zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca45   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 0     |
		| Detail 2    | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: MoveDetailValueRequired
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca46. Save with Description too large in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca46   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description                                         | Amount | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: TooLargeData
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change



Scenario: Ca51. Save with disabled Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca51   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a disabled Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: DisabledCategory
	And the move will not be saved
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: Ca52. Save with closed AccountOut (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca52   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has a closed Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this core error: ClosedAccount
	And the move will not be saved

Scenario: Ca53. Save with closed AccountOut (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca53   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has a closed Account In
	When I try to save the move
	Then I will receive this core error: ClosedAccount
	And the move will not be saved



Scenario: Ca91. Save with info all right (Out) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	
Scenario: Ca92. Save with info all right (In) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca92   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountIn value will change in 10
	And the month-category-accountIn value will change in 10
	And the year-category-accountIn value will change in 10
	
Scenario: Ca93. Save with info all right (Transfer) (S)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca93   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	And the accountIn value will change in 10
	And the month-category-accountIn value will change in 10
	And the year-category-accountIn value will change in 10

Scenario: Ca94. Save with info all right (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca94   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: Ca95. Save with info all right (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca95   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | 10    |
		| Detail 2    | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -20
	And the month-category-accountOut value will change in 20
	And the year-category-accountOut value will change in 20

Scenario: Ca96. Save negative (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca96   | 2012-03-31 | Out    | -10   |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: Ca97. Save negative (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca97   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail 1    | 1      | -10   |
		| Detail 2    | 1      | -10   |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -20
	And the month-category-accountOut value will change in 20
	And the year-category-accountOut value will change in 20

Scenario: Ca98. Save with exactly length in Description of Detail (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca98   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description                                        | Amount | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: Ca99. Save with exactly length in Description (S)
	Given I have this move to create
		| Description                                        | Date       | Nature | Value |
		| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: Ca9A. Save with details with same description (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9A   | 2012-03-31 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Move Ca9A   | 1      | 10    |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: Ca9B. Save with e-mail sender system out
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9B   | 2014-03-22 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move with e-mail system out
	Then I will receive no core error
	And I will receive the notification
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	
Scenario: Ca9C. Save with e-mail sender system ok
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9C   | 2014-03-22 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move with e-mail system ok
	Then I will receive no core error
	And I will receive no notification
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	
Scenario: Ca9D. Save with e-mail sender system ok and without category
	Given I disable Categories use
	And I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9D   | 2014-03-23 | Out    | 10    |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move with e-mail system ok
	Then I will receive no core error
	And I will receive no notification
	And the move will be saved
	And the accountOut value will change in -10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10



Scenario: Ca9E. Save with decimals
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9E   | 2014-12-30 | Out    | 9.45  |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -9.45
	And the month-category-accountOut value will change in 9.45
	And the year-category-accountOut value will change in 9.45

Scenario: Ca9F. Save with decimals in details
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca9F   | 2014-12-30 | Out    |       |
	And the move has this details
		| Description | Amount | Value |
		| Detail Ca9F | 1      | 9.45  |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no core error
	And the move will be saved
	And the accountOut value will change in -9.45
	And the month-category-accountOut value will change in 9.45
	And the year-category-accountOut value will change in 9.45
