Feature: Ea. Create move

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have two accounts
		And I have a category

Scenario: Ea01. Save without Description
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

Scenario: Ea02. Save without Date
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

Scenario: Ea03. Save with future Date
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

Scenario: Ea04. Save without Category
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

Scenario: Ea05. Save with unknown Category
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

Scenario: Ea06. Save with Description too large
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

Scenario: Ea07. Save with (Nature: Out) (AccountOut:No) (AccountIn:No)
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

Scenario: Ea08. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes)
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

Scenario: Ea09. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes)
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

Scenario: Ea10. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No)
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

Scenario: Ea11. Save with (Nature: In) (AccountOut:No) (AccountIn:No)
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

Scenario: Ea12. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes)
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

Scenario: Ea13. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No)
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

Scenario: Ea14. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown)
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

Scenario: Ea15. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No)
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

Scenario: Ea16. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes)
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

Scenario: Ea17. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No)
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

Scenario: Ea18. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown)
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

Scenario: Ea19. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes)
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

Scenario: Ea20. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out)
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

Scenario: Ea21. Save without Value or Details
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

Scenario: Ea22. Save with Value zero and no Details
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

Scenario: Ea23. Save without value and without Description in Detail
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

Scenario: Ea24. Save without value and with Amount zero in Detail
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

Scenario: Ea25. Save without value and with Value zero in Detail
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

Scenario: Ea26. Save with Description too large in Detail
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

Scenario: Ea27. Save with disabled Category
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

Scenario: Ea28. Save with closed AccountOut
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

Scenario: Ea29. Save with closed AccountIn
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

Scenario: Ea30. Save with info all right (Out)
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

Scenario: Ea31. Save with info all right (In)
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

Scenario: Ea32. Save with info all right (Transfer)
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

Scenario: Ea33. Save with info all right (value)
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

Scenario: Ea34. Save with info all right (details)
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

Scenario: Ea35. Save negative (value)
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

Scenario: Ea36. Save negative (details)
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

Scenario: Ea37. Save with exactly length in Description of Detail
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

Scenario: Ea38. Save with exactly length in Description
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

Scenario: Ea39. Save with details with same description
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

Scenario: Ea40. Save with e-mail sender system out
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

Scenario: Ea41. Save with e-mail sender system ok
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

Scenario: Ea42. Save with e-mail sender system ok and without category
	Given these settings
			| UseCategories |
			| false         |
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

Scenario: Ea43. Save with decimals
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

Scenario: Ea44. Save with decimals in details
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
Scenario: Ea45. Save move fix account begin date
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

Scenario: Ea46. Not save if user is marked for deletion
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

Scenario: Ea47. Not save if user requested wipe
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

Scenario: Ea48. Not save if not signed last contract
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


Scenario: Ea49. Save move transfer with unique value for same currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   | Value |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out BRL
		And it has an Account In BRL
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10
		And the accountIn value will change in 10
		And the month-category-accountIn value will change in 10
		And the year-category-accountIn value will change in 10

Scenario: Ea50. Save move transfer with unique value for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   | Value |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 10    |
		And it has no Details
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea51. Save move transfer with conversion for same currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 1     | 5          |
		And it has no Details
		And it has a Category
		And it has an Account Out BRL
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsSameCurrencyConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea52. Save move transfer with conversion for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 1     | 5          |
		And it has no Details
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 5
		And the month-category-accountIn value will change in 5
		And the year-category-accountIn value will change in 5

Scenario: Ea53. Save move transfer with conversion for disabled use currency
	Given these settings
			| UseCurrency |
			| false       |
		And I have this move to create
			| Description         | Date       | Nature   | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 1     | 5          |
		And it has no Details
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: UseCurrencyDisabled
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea54. Save move out with conversion
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | Out    | 1     | 5          |
		And it has no Details
		And it has a Category
		And it has an Account Out BRL
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Ea55. Save move in with conversion
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | In     | 1     | 5          |
		And it has no Details
		And it has a Category
		And it has no Account Out
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the move will not be saved
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change


Scenario: Ea56. Save move transfer with unique detailed value for same currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value |
			| Detail      | 1      | 10    |
		And it has a Category
		And it has an Account Out BRL
		And it has an Account In BRL
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the accountOut value will change in -10
		And the month-category-accountOut value will change in 10
		And the year-category-accountOut value will change in 10
		And the accountIn value will change in 10
		And the month-category-accountIn value will change in 10
		And the year-category-accountIn value will change in 10

Scenario: Ea57. Save move transfer with unique detailed value for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value |
			| Detail      | 1      | 10    |
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea58. Save move transfer with detailed conversion for same currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 5          |
		And it has a Category
		And it has an Account Out BRL
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsSameCurrencyConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea59. Save move transfer with detailed conversion for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 5          |
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive no core error
		And the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 5
		And the month-category-accountIn value will change in 5
		And the year-category-accountIn value will change in 5

Scenario: Ea60. Save move transfer with detailed conversion for disabled use currency
	Given these settings
			| UseCurrency |
			| false       |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 5          |
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: UseCurrencyDisabled
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea61. Save move out with detailed conversion
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature |
			| Move {scenarioCode} | 2024-04-09 | Out    |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 5          |
		And it has a Category
		And it has an Account Out BRL
		And it has no Account In
	When I try to save the move
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea62. Save move in with detailed conversion
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature |
			| Move {scenarioCode} | 2024-04-09 | In     |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 5          |
		And it has a Category
		And it has no Account Out
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change


Scenario: Ea63. Save move transfer with conversion ZERO for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   | Value | Conversion |
			| Move {scenarioCode} | 2024-04-09 | Transfer | 1     | 0          |
		And it has no Details
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Ea64. Save move transfer with detailed conversion ZERO for different currencies
	Given these settings
			| UseCurrency |
			| true        |
		And I have this move to create
			| Description         | Date       | Nature   |
			| Move {scenarioCode} | 2024-04-09 | Transfer |
		And the move has this details
			| Description | Amount | Value | Conversion |
			| Detail      | 1      | 1     | 0          |
		And it has a Category
		And it has an Account Out EUR
		And it has an Account In BRL
	When I try to save the move
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change
