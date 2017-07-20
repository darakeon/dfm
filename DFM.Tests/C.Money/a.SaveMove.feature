Feature: a. Creation of Move

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Save without Description (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		|             | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                   |
		| MoveDescriptionRequired |
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
	Then I will receive this error
		| Error            |
		| MoveDateRequired |
	And the move will not be saved

Scenario: 03. Save with future Date (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca03   | 31/03/2099 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error           |
		| MoveDateInvalid |
	And the move will not be saved

Scenario: 04. Save without Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca04   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has no Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                |
		| MoveCategoryRequired |
	And the move will not be saved

Scenario: 05. Save with unknown Category (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca05   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has an unknown Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error               |
		| MoveCategoryInvalid |
	And the move will not be saved



Scenario: 11. Save with (Nature: Out) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca11   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the move will not be saved

Scenario: 12. Save with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca12   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the move will not be saved

Scenario: 13. Save with (Nature: Out) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca13   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the move will not be saved

Scenario: 14. Save with (Nature: Out) (AccountOut:Unknown) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca14   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error        |
		| OutMoveWrong |
	And the move will not be saved



Scenario: 21. Save with (Nature: In) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca21   | 31/03/2012 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the move will not be saved

Scenario: 22. Save with (Nature: In) (AccountOut:Yes) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca22   | 31/03/2012 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the move will not be saved

Scenario: 23. Save with (Nature: In) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca23   | 31/03/2012 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the move will not be saved

Scenario: 24. Save with (Nature: In) (AccountOut:No) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca24   | 31/03/2012 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this error
		| Error       |
		| InMoveWrong |
	And the move will not be saved



Scenario: 31. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca31   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the move will not be saved

Scenario: 32. Save with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca32   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the move will not be saved

Scenario: 33. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca33   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the move will not be saved

Scenario: 34. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca34   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an unknown Account In
	When I try to save the move
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the move will not be saved

Scenario: 35. Save with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an unknown Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive this error
		| Error             |
		| TransferMoveWrong |
	And the move will not be saved

Scenario: 36. Save with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out) (E)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca35   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In equal to Out
	When I try to save the move
	Then I will receive this error
		| Error             |
		| MoveCircularTransfer |
	And the move will not be saved



Scenario: 41. Save without Value or Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca41   | 31/03/2012 | Out    |       |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                     |
		| MoveValueOrDetailRequired |
	And the move will not be saved

Scenario: 42. Save with Value zero and no Details (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca42   | 31/03/2012 | Out    | 0     |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                     |
		| MoveValueOrDetailRequired |
	And the move will not be saved

Scenario: 43. Save without value and without Description in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca43   | 31/03/2012 | Out    |       |
	And the move has this details
		| Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
		|              | 1       | 10     | Detail 2     | 1       | 10     |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                         |
		| MoveDetailDescriptionRequired |
	And the move will not be saved

Scenario: 44. Save without value and with Amount zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca44   | 31/03/2012 | Out    |       |
	And the move has this details
		| Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
		| Detail 1     | 0       | 10     | Detail 2     | 1       | 10     |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                    |
		| MoveDetailAmountRequired |
	And the move will not be saved

Scenario: 45. Save without value and with Value zero in Detail (E)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca45   | 31/03/2012 | Out    |       |
	And the move has this details
		| Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
		| Detail 1     | 1       | 0      | Detail 2     | 1       | 10     |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive this error
		| Error                   |
		| MoveDetailValueRequired |
	And the move will not be saved



Scenario: 91. Save with info all right (value - Out) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca96   | 31/03/2012 | Out    | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	
Scenario: 92. Save with info all right (value - In) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca96   | 31/03/2012 | In     | 10    |
	And it has no Details
	And it has a Category
	And it has no Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved
	
Scenario: 93. Save with info all right (value - Transfer) (S)
	Given I have this move to create
		| Description | Date       | Nature   | Value |
		| Move Ca96   | 31/03/2012 | Transfer | 10    |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has an Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved

Scenario: 94. Save with info all right (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca97   | 31/03/2012 | Out    | 10    |
	And the move has this details
		| Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
		| Detail 1     | 1       | 10     | Detail 2     | 1       | 10     |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved

Scenario: 95. Save negative (value) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca98   | 31/03/2012 | Out    | -10   |
	And it has no Details
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved

Scenario: 96. Save negative (details) (S)
	Given I have this move to create
		| Description | Date       | Nature | Value |
		| Move Ca99   | 31/03/2012 | Out    | 10    |
	And the move has this details
		| Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
		| Detail 1     | 1       | -10    | Detail 2     | 1       | 10     |
	And it has a Category
	And it has an Account Out
	And it has no Account In
	When I try to save the move
	Then I will receive no error
	And the move will be saved