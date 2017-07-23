Feature: a. Creation of Move

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Save without Description (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		|             | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: MoveDescriptionRequired
	And the move will not be saved

Scenario: 02. Save without Date (E)
	Given I have this move to create
		| Description | Date | Nature | Value |
		| Move Ca02   |      | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: MoveDateRequired
	And the move will not be saved

Scenario: 03. Save with future Date (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca03   | 2099-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: MoveDateInvalid
	And the move will not be saved

Scenario: 04. Save without Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca04   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: InvalidCategory
	And the move will not be saved

Scenario: 05. Save with unknown Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca05   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has an unknown Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: InvalidCategory
	And the move will not be saved



Scenario: 11. Save with (Nature: Out) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca11   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: OutMoveWrong
	And the move will not be saved

Scenario: 12. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca12   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error: OutMoveWrong
	And the move will not be saved

Scenario: 13. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca13   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error: OutMoveWrong
	And the move will not be saved

Scenario: 14. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca14   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: InvalidAccount
	And the move will not be saved



Scenario: 21. Save with (Nature: In) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca21   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: InMoveWrong
	And the move will not be saved

Scenario: 22. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca22   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error: InMoveWrong
	And the move will not be saved

Scenario: 23. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca23   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: InMoveWrong
	And the move will not be saved

Scenario: 24. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca24   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this error: InvalidAccount
	And the move will not be saved



Scenario: 31. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca31   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: TransferMoveWrong
	And the move will not be saved

Scenario: 32. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca32   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error: TransferMoveWrong
	And the move will not be saved

Scenario: 33. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca33   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: TransferMoveWrong
	And the move will not be saved

Scenario: 34. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca34   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this error: InvalidAccount
	And the move will not be saved

Scenario: 35. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error: InvalidAccount
	And the move will not be saved

Scenario: 36. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca36   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In equal to Out
	When I try to save the move
	Then I will receive this error: MoveCircularTransfer
	And the move will not be saved



Scenario: 41. Save without Value or Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca41   | 2012-03-31 | Out    |       |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: MoveValueOrDetailRequired
	And the move will not be saved

Scenario: 42. Save with Value zero and no Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca42   | 2012-03-31 | Out    | 0     |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error: MoveValueOrDetailRequired
	And the move will not be saved

Scenario: 43. Save without value and without Description in Detail (E)
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
	Then I will receive this error: MoveDetailDescriptionRequired
	And the move will not be saved

Scenario: 44. Save without value and with Amount zero in Detail (E)
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
	Then I will receive this error: MoveDetailAmountRequired
	And the move will not be saved

Scenario: 45. Save without value and with Value zero in Detail (E)
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
	Then I will receive this error: MoveDetailValueRequired
	And the move will not be saved



Scenario: 91. Save with info all right (Out) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca91   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	
Scenario: 92. Save with info all right (In) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca92   | 2012-03-31 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	And the accountIn value will increase in 10
	And the month-category-accountIn value will change in 10
	And the year-category-accountIn value will change in 10
	
Scenario: 93. Save with info all right (Transfer) (S)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca93   | 2012-03-31 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10
	And the accountIn value will increase in 10
	And the month-category-accountIn value will change in 10
	And the year-category-accountIn value will change in 10

Scenario: 94. Save with info all right (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca94   | 2012-03-31 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: 95. Save with info all right (details) (S)
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
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 20
	And the month-category-accountOut value will change in 20
	And the year-category-accountOut value will change in 20

Scenario: 96. Save negative (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca96   | 2012-03-31 | Out    | -10   |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 10
	And the month-category-accountOut value will change in 10
	And the year-category-accountOut value will change in 10

Scenario: 97. Save negative (details) (S)
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
	Then I will receive no error
	And the move will be saved
	And the accountOut value will decrease in 20
	And the month-category-accountOut value will change in 20
	And the year-category-accountOut value will change in 20
