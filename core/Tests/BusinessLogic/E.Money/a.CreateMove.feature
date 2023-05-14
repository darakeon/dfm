Feature: Ca. Create move

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Ca01. Save without Description
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

Scenario: Ca02. Save without Date
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

Scenario: Ca03. Save with future Date
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

Scenario: Ca04. Save without Category
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

Scenario: Ca05. Save with unknown Category
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

Scenario: Ca06. Save with Description too large
	Given I have this move to create
			| Description                                         | Date       | Nature | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: TooLargeMoveDescription
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Ca07. Save with (Nature: Out) (AccountOut:No) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca07   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: OutMoveWrong
		And the move will not be saved

Scenario: Ca08. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca08   | 2012-03-31 | Out    | 10    |
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

Scenario: Ca09. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca09   | 2012-03-31 | Out    | 10    |
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

Scenario: Ca10. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca10   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an unknown Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
		And the move will not be saved

Scenario: Ca11. Save with (Nature: In) (AccountOut:No) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca11   | 2012-03-31 | In     | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: InMoveWrong
		And the move will not be saved

Scenario: Ca12. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca12   | 2012-03-31 | In     | 10    |
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

Scenario: Ca13. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca13   | 2012-03-31 | In     | 10    |
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

Scenario: Ca14. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca14   | 2012-03-31 | In     | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an unknown Account In
	When I try to save the move
	Then I will receive this core error: InvalidAccount
		And the move will not be saved

Scenario: Ca15. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca15   | 2012-03-31 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: TransferMoveWrong
		And the move will not be saved

Scenario: Ca16. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca16   | 2012-03-31 | Transfer | 10    |
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

Scenario: Ca17. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca17   | 2012-03-31 | Transfer | 10    |
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

Scenario: Ca18. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca18   | 2012-03-31 | Transfer | 10    |
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

Scenario: Ca19. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca19   | 2012-03-31 | Transfer | 10    |
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

Scenario: Ca20. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca20   | 2012-03-31 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In equal to Out
	When I try to save the move
	Then I will receive this core error: CircularTransfer
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ca21. Save without Value or Details
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca21   | 2012-03-31 | Out    |       |
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

Scenario: Ca22. Save with Value zero and no Details
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca22   | 2012-03-31 | Out    | 0     |
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

Scenario: Ca23. Save without value and without Description in Detail
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca23   | 2012-03-31 | Out    |       |
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

Scenario: Ca24. Save without value and with Amount zero in Detail
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca24   | 2012-03-31 | Out    |       |
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

Scenario: Ca25. Save without value and with Value zero in Detail
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca25   | 2012-03-31 | Out    |       |
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

Scenario: Ca26. Save with Description too large in Detail
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca26   | 2012-03-31 | Out    |       |
		And the move has this details
			| Description                                         | Amount | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: TooLargeDetailDescription
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Ca27. Save with disabled Category
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca27   | 2012-03-31 | Out    | 10    |
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

Scenario: Ca28. Save with closed AccountOut
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca28   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has a closed Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: ClosedAccount
		And the move will not be saved

Scenario: Ca29. Save with closed AccountIn
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca29   | 2012-03-31 | In     | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has a closed Account In
	When I try to save the move
	Then I will receive this core error: ClosedAccount
		And the move will not be saved

Scenario: Ca30. Save with info all right (Out)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca30   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca31. Save with info all right (In)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca31   | 2012-03-31 | In     | 10    |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountIn value will change in 10
		And the month-category-accountIn value will change in 10
		And the year-category-accountIn value will change in 10

Scenario: Ca32. Save with info all right (Transfer)
	Given I have this move to create
			| Description | Date       | Nature   | Value |
			| Move Ca32   | 2012-03-31 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10
		And the accountIn value will change in 10
		And the month-category-accountIn value will change in 10
		And the year-category-accountIn value will change in 10

Scenario: Ca33. Save with info all right (value)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca33   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca34. Save with info all right (details)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca34   | 2012-03-31 | Out    |       |
		And the move has this details
			| Description | Amount | Value |
			| Detail 1    | 3      | 27    |
			| Detail 2    | 9      | 24    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 297
		And the accountOut value will change in -297
		And the month-category-accountOut value will change in 297
		And the year-category-accountOut value will change in 297

Scenario: Ca35. Save negative (value)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca35   | 2012-03-31 | Out    | -10   |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca36. Save negative (details)
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca36   | 2012-03-31 | Out    |       |
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
		And the move value will be 20
		And the accountOut value will change in -20
		And the month-category-accountOut value will change in 20
		And the year-category-accountOut value will change in 20

Scenario: Ca37. Save with exactly length in Description of Detail
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca37   | 2012-03-31 | Out    |       |
		And the move has this details
			| Description                                        | Amount | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca38. Save with exactly length in Description
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
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca39. Save with details with same description
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca39   | 2012-03-31 | Out    |       |
		And the move has this details
			| Description | Amount | Value |
			| Move Ca39   | 1      | 10    |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca40. Save with e-mail sender system out
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca40   | 2014-03-22 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move with e-mail system out
	Then I will receive no core error
		And I will receive the notification
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca41. Save with e-mail sender system ok
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca41   | 2014-03-22 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move with e-mail system ok
	Then I will receive no core error
		And I will receive no notification
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10
		And the move e-mail will have an unsubscribe link

Scenario: Ca42. Save with e-mail sender system ok and without category
	Given I disable Categories use
		And I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca42   | 2014-03-23 | Out    | 10    |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move with e-mail system ok
	Then I will receive no core error
		And I will receive no notification
		And the move will be saved
		And the move value will be 10
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10

Scenario: Ca43. Save with decimals
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca43   | 2014-12-30 | Out    | 9.45  |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 9.45
		And the accountOut value will change in -9.45
		And the month-category-accountOut value will change in 9.45
		And the year-category-accountOut value will change in 9.45

Scenario: Ca44. Save with decimals in details
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca44   | 2014-12-30 | Out    |       |
		And the move has this details
			| Description | Amount | Value |
			| Detail Ca44 | 1      | 9.45  |
		And it has a Category
		And it has an Account Out
		And it has no Account In
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the move value will be 9.45
		And the accountOut value will change in -9.45
		And the month-category-accountOut value will change in 9.45
		And the year-category-accountOut value will change in 9.45

# because begin date is used to get the date of the start of reports
Scenario: Ca45. Save move fix account begin date
	Given I have this move to create
			| Description   | Date       | Nature   | Value |
			| Move Ca45 Old | 1986-05-04 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the move
	Then I will receive no core error
		And the accountOut begin date will be 1986-05-04
		And the accountIn begin date will be 1986-05-04
	Given I have this move to create
			| Description   | Date       | Nature   | Value |
			| Move Ca45 New | 1986-12-01 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has an Account In
	When I try to save the move
	Then I will receive no core error
		And the accountOut begin date will be 1986-05-04
		And the accountIn begin date will be 1986-05-04

Scenario: Ca46. Not save if user is marked for deletion
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca46   | 2021-05-16 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But the user is marked for deletion
	When I try to save the move
	Then I will receive this core error: UserDeleted

Scenario: Ca47. Not save if user requested wipe
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca47   | 2021-06-26 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But the user asked data wipe
	When I try to save the move
	Then I will receive this core error: UserAskedWipe

Scenario: Ca48. Not save if not signed last contract
	Given I have this move to create
			| Description | Date       | Nature | Value |
			| Move Ca48   | 2012-03-31 | Out    | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		But there is a new contract
	When I try to save the move
	Then I will receive this core error: NotSignedLastContract
