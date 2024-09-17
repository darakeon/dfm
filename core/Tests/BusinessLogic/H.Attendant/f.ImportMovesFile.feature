Feature: Hf. Import moves file

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I open the account Account Out
		And I open the account Account In
		And I have these accounts
			| Name                 | Currency |
			| Account Out EUR      | EUR      |
			| Account In BRL       | BRL      |
			| ABCDEFGHIJKLMNOPQRST | EUR      |
			| GHIJKLMNOPQRSTUVWXYZ | BRL      |
		And I have a category
		And I have this category
			| Name                 |
			| ABCDEFGHIJKLMNOPQRST |
		And I enable the category Category

Scenario: Hf01. Import with user marked for deletion
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But the user is marked for deletion
	When import moves file
	Then I will receive this core error: UserDeleted
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf02. Import with user requested wipe
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But the user asked data wipe
	When import moves file
	Then I will receive this core error: UserAskedWipe
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf03. Import without sign last contract
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But there is a new contract
	When import moves file
	Then I will receive this core error: NotSignedLastContract
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf04. Import empty
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
	When import moves file
	Then I will receive this core error: EmptyArchive
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf05. Import with unknown column
	Given a moves file with this content
			| Description         | Magic              | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | make me earn money | 2024-04-29 | Category | Transfer | Account Out | Account In |       |
	When import moves file
	Then I will receive this core error: InvalidArchiveColumn
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf06. Import with 30 details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 | Description4 | Amount4 | Value4 | Description5 | Amount5 | Value5 | Description6 | Amount6 | Value6 | Description7 | Amount7 | Value7 | Description8 | Amount8 | Value8 | Description9 | Amount9 | Value9 | Description10 | Amount10 | Value10 | Description11 | Amount11 | Value11 | Description12 | Amount12 | Value12 | Description13 | Amount13 | Value13 | Description14 | Amount14 | Value14 | Description15 | Amount15 | Value15 | Description16 | Amount16 | Value16 | Description17 | Amount17 | Value17 | Description18 | Amount18 | Value18 | Description19 | Amount19 | Value19 | Description20 | Amount20 | Value20 | Description21 | Amount21 | Value21 | Description22 | Amount22 | Value22 | Description23 | Amount23 | Value23 | Description24 | Amount24 | Value24 | Description25 | Amount25 | Value25 | Description26 | Amount26 | Value26 | Description27 | Amount27 | Value27 | Description28 | Amount28 | Value28 | Description29 | Amount29 | Value29 | Description30 | Amount30 | Value30 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D1           | 1       | 101.00 | D2           | 2       | 202.00 | D3           | 3       | 303.00 | D4           | 4       | 404.00 | D5           | 5       | 505.00 | D6           | 6       | 606.00 | D7           | 7       | 707.00 | D8           | 8       | 808.00 | D9           | 9       | 909.00 | D10           | 10       | 1010.00 | D11           | 11       | 1111.00 | D12           | 12       | 1212.00 | D13           | 13       | 1313.00 | D14           | 14       | 1414.00 | D15           | 15       | 1515.00 | D16           | 16       | 1616.00 | D17           | 17       | 1717.00 | D18           | 18       | 1818.00 | D19           | 19       | 1919.00 | D20           | 20       | 2020.00 | D21           | 21       | 2121.00 | D22           | 22       | 2222.00 | D23           | 23       | 2323.00 | D24           | 24       | 2424.00 | D25           | 25       | 2525.00 | D26           | 26       | 2626.00 | D27           | 27       | 2727.00 | D28           | 28       | 2828.00 | D29           | 29       | 2929.00 | D30           | 30       | 3030.00 |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf07. Import with 31 details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 | Description4 | Amount4 | Value4 | Description5 | Amount5 | Value5 | Description6 | Amount6 | Value6 | Description7 | Amount7 | Value7 | Description8 | Amount8 | Value8 | Description9 | Amount9 | Value9 | Description10 | Amount10 | Value10 | Description11 | Amount11 | Value11 | Description12 | Amount12 | Value12 | Description13 | Amount13 | Value13 | Description14 | Amount14 | Value14 | Description15 | Amount15 | Value15 | Description16 | Amount16 | Value16 | Description17 | Amount17 | Value17 | Description18 | Amount18 | Value18 | Description19 | Amount19 | Value19 | Description20 | Amount20 | Value20 | Description21 | Amount21 | Value21 | Description22 | Amount22 | Value22 | Description23 | Amount23 | Value23 | Description24 | Amount24 | Value24 | Description25 | Amount25 | Value25 | Description26 | Amount26 | Value26 | Description27 | Amount27 | Value27 | Description28 | Amount28 | Value28 | Description29 | Amount29 | Value29 | Description30 | Amount30 | Value30 | Description31 | Amount31 | Value31 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D1           | 1       | 101.00 | D2           | 2       | 202.00 | D3           | 3       | 303.00 | D4           | 4       | 404.00 | D5           | 5       | 505.00 | D6           | 6       | 606.00 | D7           | 7       | 707.00 | D8           | 8       | 808.00 | D9           | 9       | 909.00 | D10           | 10       | 1010.00 | D11           | 11       | 1111.00 | D12           | 12       | 1212.00 | D13           | 13       | 1313.00 | D14           | 14       | 1414.00 | D15           | 15       | 1515.00 | D16           | 16       | 1616.00 | D17           | 17       | 1717.00 | D18           | 18       | 1818.00 | D19           | 19       | 1919.00 | D20           | 20       | 2020.00 | D21           | 21       | 2121.00 | D22           | 22       | 2222.00 | D23           | 23       | 2323.00 | D24           | 24       | 2424.00 | D25           | 25       | 2525.00 | D26           | 26       | 2626.00 | D27           | 27       | 2727.00 | D28           | 28       | 2828.00 | D29           | 29       | 2929.00 | D30           | 30       | 3030.00 | D31           | 31       | 3131.00 |
	When import moves file
	Then I will receive this core error: InvalidArchiveColumn
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf08. Import without Description
	Given a moves file with this content
			| Description | Date       | Category | Nature   | Out         | In         | Value |
			|             | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: MoveDescriptionRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf09. Import without Date
	Given a moves file with this content
			| Description         | Date | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} |      | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: MoveDateRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf10. Import with invalid Date
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-31 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: MoveDateInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf11. Import with future Date
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 3024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: MoveDateInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf12. Import without Category but using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 |          | Transfer | Account Out | Account In | 1.00  |
		And these settings
			| UseCategories |
			| true          |
	When import moves file
	Then I will receive this core error: InvalidCategory
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf13. Import with Category but not using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		And these settings
			| UseCategories |
			| false         |
	When import moves file
	Then I will receive this core error: CategoriesDisabled
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf14. Import with unknown Category
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Unknown  | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: InvalidCategory
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf15. Import with Description too large
	Given a moves file with this content
			| Description                                         | Date       | Category | Nature   | Out         | In         | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: TooLargeMoveDescription
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf16. Import with (Nature: Out) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    |     |    | 1.00  |
	When import moves file
	Then I will receive this core error: OutMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf17. Import without Nature
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category |        |     |    | 1.00  |
	When import moves file
	Then I will receive this core error: MoveNatureRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf18. Import with invalid Nature
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Alien  |     |    | 1.00  |
	When import moves file
	Then I will receive this core error: MoveNatureInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf19. Import with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: OutMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf20. Import with (Nature: Out) (AccountOut:No) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    |     | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: OutMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf21. Import with (Nature: Out) (AccountOut:Unknown) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out     | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Unknown |    | 1.00  |
	When import moves file
	Then I will receive this core error: InvalidAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf22. Import with (Nature: In) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     |    | 1.00  |
	When import moves file
	Then I will receive this core error: InMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf23. Import with (Nature: In) (AccountOut:Yes) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: InMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf24. Import with (Nature: In) (AccountOut:Yes) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     | Account Out |    | 1.00  |
	When import moves file
	Then I will receive this core error: InMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf25. Import with (Nature: In) (AccountOut:No) (AccountIn:Unknown)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In      | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Unknown | 1.00  |
	When import moves file
	Then I will receive this core error: InvalidAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf26. Import with (Nature: Transfer) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer |             |            | 1.00  |
	When import moves file
	Then I will receive this core error: TransferMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf27. Import with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer |     | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: TransferMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf28. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out |    | 1.00  |
	When import moves file
	Then I will receive this core error: TransferMoveWrong
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf29. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In      | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Unknown | 1.00  |
	When import moves file
	Then I will receive this core error: InvalidAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf30. Import with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out     | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Unknown | Account In | 1.00  |
	When import moves file
	Then I will receive this core error: InvalidAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf31. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In          | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account Out | 1.00  |
	When import moves file
	Then I will receive this core error: CircularTransfer
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf32. Import without Value nor Details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       |
	When import moves file
	Then I will receive this core error: MoveValueOrDetailRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf33. Import with Value zero and no Details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 0.00  |
	When import moves file
	Then I will receive this core error: MoveValueOrDetailRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf34. Import with invalid Value
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value  |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | MMXXIV |
	When import moves file
	Then I will receive this core error: MoveValueInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf35. Import without Description in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       |              | 1       | 1.00   |
	When import moves file
	Then I will receive this core error: MoveDetailDescriptionRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf36. Import with Amount zero in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 0       | 1.00   |
	When import moves file
	Then I will receive this core error: MoveDetailAmountRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf37. Import with Amount invalid in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | I       | 1.00   |
	When import moves file
	Then I will receive this core error: MoveDetailAmountInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf38. Import without Amount in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            |         | 1.00   |
	When import moves file
	Then I will receive this core error: MoveDetailAmountRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf39. Import with Value zero in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 0.00   |
	When import moves file
	Then I will receive this core error: MoveDetailValueRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf40. Import without Value in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       |        |
	When import moves file
	Then I will receive this core error: MoveDetailValueRequired
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf41. Import with Value invalid in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | XXVII  |
	When import moves file
	Then I will receive this core error: MoveDetailValueInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf42. Import with Description too large in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1                                        | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1       | 1.00   |
	When import moves file
	Then I will receive this core error: TooLargeDetailDescription
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf43. Import with disabled Category
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But I disable the category Category
	When import moves file
	Then I will receive this core error: DisabledCategory
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf44. Import with closed AccountOut
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But I close the account Account Out
	When import moves file
	Then I will receive this core error: ClosedAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf45. Import with closed AccountIn
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But I close the account Account In
	When import moves file
	Then I will receive this core error: ClosedAccount
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf46. Import with info all right (Out)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out |    | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf47. Import with info all right (In)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf48. Import with info all right (Transfer)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf49. Import negative (value)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | -1.00 |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf50. Import negative (details)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | -1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf51. Import with exactly length in Description of Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1                                       | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1       | 1.00   |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf52. Import with exactly length in Description
	Given a moves file with this content
			| Description                                        | Date       | Category | Nature   | Out         | In         | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf53. Import with details with same Description
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | Same         | 1       | 1.00   | Same         | 1       | 1.00   |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf54. Import with decimals
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.10  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf55. Import with decimals in details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.10   |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf56. Import move transfer with unique value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf57. Import move transfer with unique value for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1.00  |            |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf58. Import move transfer with conversion for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  | 5.00       |
	When import moves file
	Then I will receive this core error: AccountsSameCurrencyConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf59. Import move transfer with conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1.00  | 5.00       |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf60. Import move transfer with conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1.00  | 5.00       |
		But these settings
			| UseCurrency |
			| false       |
	When import moves file
	Then I will receive this core error: UseCurrencyDisabled
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf61. Import move out with conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    | 1.00  | 5.00       |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf62. Import move in with conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL | 1.00  | 5.00       |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf63. Import move transfer with conversion ZERO for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1.00  | 0.00       |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf64. Import move out with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    | 1.00  |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf65. Import move in with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL | 1.00  |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf66. Import move transfer with unique detailed value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.00   |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf67. Import move transfer with unique detailed value for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1.00   |             |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf68. Import move transfer with detailed conversion for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.00   | 5.00        |
	When import moves file
	Then I will receive this core error: AccountsSameCurrencyConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf69. Import move transfer with detailed conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1.00   | 5.00        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf70. Import move transfer with detailed conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1.00   | 5.00        |
		But these settings
			| UseCurrency |
			| false       |
	When import moves file
	Then I will receive this core error: UseCurrencyDisabled
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf71. Import move out with detailed conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    |       | D            | 1       | 1.00   | 5.00        |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf72. Import move in with detailed conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL |       | D            | 1       | 1.00   | 5.00        |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf73. Import move transfer with detailed conversion ZERO for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1.00   | 0.00        |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf74. Import move transfer with detailed conversion invalid for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1.00   | V           |
	When import moves file
	Then I will receive this core error: MoveDetailConversionInvalid
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf75. Import move out with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    |       | D            | 1       | 1.00   |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf76. Import move in with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL |       | D            | 1       | 1.00   |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf77. Error in multiple lines
	Given a moves file with this content
			| Description           | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} 1 | 3024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
			| Move {scenarioCode} 2 | 2024-04-29 | Category | Alien    | Account Out | Account In | 1.00  |
			| Move {scenarioCode} 3 | 2024-04-29 | Category | Transfer |             |            | 1.00  |
			| Move {scenarioCode} 4 | 2024-04-29 | Alien    | Transfer | Account Out | Account In | 1.00  |
			| Move {scenarioCode} 5 | 2024-04-29 | Category | Transfer | Alien       | Account In | 1.00  |
			| Move {scenarioCode} 6 | 2024-04-29 | Category | Transfer | Account Out | Alien      | 1.00  |
	When import moves file
	Then I will receive these core errors
			| Error             |
			| MoveDateInvalid   |
			| MoveNatureInvalid |
			| TransferMoveWrong |
			| InvalidCategory   |
			| InvalidAccount    |
			| InvalidAccount    |
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf78. Import with empty details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 |
			| Move {scenarioCode} | 2024-06-05 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.00   | D            | 1       | 1.00   |              |         |        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf79. Without explicit Nature
	Given a moves file with this content
			| Description         | Date       | Category | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-05 | Category | Account Out | Account In | 1.00  |
			| Move {scenarioCode} | 2024-06-05 | Category |             | Account In | 1.00  |
			| Move {scenarioCode} | 2024-06-05 | Category | Account Out |            | 1.00  |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf80. Allowed file size and lines
	Given a moves file with allowed file size and lines
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued

Scenario: Hf81. Not allowed file size
	Given a moves file with not allowed file size
	When import moves file
	Then I will receive this core error: InvalidArchiveSize
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf82. Not allowed file lines
	Given a moves file with not allowed file lines
	When import moves file
	Then I will receive this core error: InvalidArchiveLines
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf83. Is not CSV
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But the file is not CSV
	When import moves file
	Then I will receive this core error: InvalidArchiveType
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf84. Too long file name
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.00  |
		But the file name is 257 characters long
	When import moves file
	Then I will receive this core error: InvalidArchiveName
		And the pre-import data will not be recorded
		And the lines will not be queued

Scenario: Hf85. Import exported file
	Given I have moves of
			| Description                                      | Date       | Category | Nature   | Out             | In             | Value | Conversion | Detail |
			| Sample Move Out                                  | 2024-09-16 | Category | Out      | Account Out     |                | 1     |            |        |
			| Sample Move Out with Details                     | 2024-09-16 | Category | Out      | Account Out     |                | 1     |            | D1     |
			| Sample Move In                                   | 2024-09-16 | Category | In       |                 | Account In     | 1     |            |        |
			| Sample Move In with Details                      | 2024-09-16 | Category | In       |                 | Account In     | 1     |            | D1     |
			| Sample Move Transfer                             | 2024-09-16 | Category | Transfer | Account Out     | Account In     | 1     |            |        |
			| Sample Move Transfer with Details                | 2024-09-16 | Category | Transfer | Account Out     | Account In     | 1     |            | D1     |
			| Sample Move Transfer with Conversion             | 2024-09-16 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 10         |        |
			| Sample Move Transfer with Conversion and Details | 2024-09-16 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 10         | D1     |
		And order start date 2024-09-16
		And order end date 2024-09-16
		And order account account_out
		And order account account_in
		And order account account_out_eur
		And order account account_in_brl
		And order category Category
		And an export is ordered
		And robot export the order
		And test user login
		And the order file is chosen to import
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And the lines will be queued
