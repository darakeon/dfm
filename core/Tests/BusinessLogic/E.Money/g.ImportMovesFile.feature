Feature: Eg. Import moves file

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |
		And I have two accounts
		And I have a category

Scenario: Eg01. Import with user marked for deletion
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
		But the user is marked for deletion
	When import moves file
	Then I will receive this core error: UserDeleted

Scenario: Eg02. Import with user requested wipe
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
		But the user asked data wipe
	When import moves file
	Then I will receive this core error: UserAskedWipe

Scenario: Eg03. Import without sign last contract
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
		But there is a new contract
	When import moves file
	Then I will receive this core error: NotSignedLastContract

Scenario: Eg04. Import empty
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
		But there is a new contract
	When import moves file
	Then I will receive this core error: CSVInvalidFile

Scenario: Eg07. Import with unknown column
	Given a moves file with this content
			| Description         | Magic              | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | make me earn money | 2024-04-29 | Category | Transfer | Account Out | Account In |       |
	When import moves file
	Then I will receive this core error: CSVInvalidColumn

Scenario: Eg09. Import with 20 details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 | Description4 | Amount4 | Value4 | Description5 | Amount5 | Value5 | Description6 | Amount6 | Value6 | Description7 | Amount7 | Value7 | Description8 | Amount8 | Value8 | Description9 | Amount9 | Value9 | Description10 | Amount10 | Value10 | Description11 | Amount11 | Value11 | Description12 | Amount12 | Value12 | Description13 | Amount13 | Value13 | Description14 | Amount14 | Value14 | Description15 | Amount15 | Value15 | Description16 | Amount16 | Value16 | Description17 | Amount17 | Value17 | Description18 | Amount18 | Value18 | Description19 | Amount19 | Value19 | Description20 | Amount20 | Value20 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D1           | 1       | 101    | D2           | 2       | 202    | D3           | 3       | 303    | D4           | 4       | 404    | D5           | 5       | 505    | D6           | 6       | 606    | D7           | 7       | 707    | D8           | 8       | 808    | D9           | 9       | 909    | D10           | 10       | 1010    | D11           | 11       | 1111    | D12           | 12       | 1212    | D13           | 13       | 1313    | D14           | 14       | 1414    | D15           | 15       | 1515    | D16           | 16       | 1616    | D17           | 17       | 1717    | D18           | 18       | 1818    | D19           | 19       | 1919    | D20           | 20       | 2020    |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg10. Import with 21 details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 | Description4 | Amount4 | Value4 | Description5 | Amount5 | Value5 | Description6 | Amount6 | Value6 | Description7 | Amount7 | Value7 | Description8 | Amount8 | Value8 | Description9 | Amount9 | Value9 | Description10 | Amount10 | Value10 | Description11 | Amount11 | Value11 | Description12 | Amount12 | Value12 | Description13 | Amount13 | Value13 | Description14 | Amount14 | Value14 | Description15 | Amount15 | Value15 | Description16 | Amount16 | Value16 | Description17 | Amount17 | Value17 | Description18 | Amount18 | Value18 | Description19 | Amount19 | Value19 | Description20 | Amount20 | Value20 | Description21 | Amount21 | Value21 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D            | 1       | 1      | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       | D             | 1        | 1       |
	When import moves file
	Then I will receive this core error: CSVInvalidColumn

Scenario: Eg11. Import without Description
	Given a moves file with this content
			| Description | Date       | Category | Nature   | Out         | In         | Value |
			|             | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: MoveDescriptionRequired

Scenario: Eg12. Import without Date
	Given a moves file with this content
			| Description         | Date | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} |      | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: MoveDateRequired

Scenario: Eg13. Import with future Date
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 3024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: MoveDateInvalid

Scenario: Eg14. Import without Category but using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 |          | Transfer | Account Out | Account In | 1     |
		And these settings
			| UseCategory |
			| true        |
	When import moves file
	Then I will receive this core error: InvalidCategory

Scenario: Eg14. Import with Category but not using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
		And these settings
			| UseCategory |
			| false       |
	When import moves file
	Then I will receive this core error: CategoriesDisabled

Scenario: Eg15. Import with unknown Category
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Unknown  | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: InvalidCategory

Scenario: Eg16. Import with Description too large
	Given a moves file with this content
			| Description                                         | Date       | Category | Nature   | Out         | In         | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: TooLargeMoveDescription

Scenario: Eg17. Import with (Nature: Out) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    |     |    | 1     |
	When import moves file
	Then I will receive this core error: OutMoveWrong

Scenario: Eg18. Import with (Nature: Out) (AccountOut:Yes) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: OutMoveWrong

Scenario: Eg19. Import with (Nature: Out) (AccountOut:No) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    |     | Account In | 1     |
	When import moves file
	Then I will receive this core error: OutMoveWrong

Scenario: Eg20. Import with (Nature: Out) (AccountOut:Unknown) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out     | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Unknown |    | 1     |
	When import moves file
	Then I will receive this core error: InvalidAccount

Scenario: Eg21. Import with (Nature: In) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     |    | 1     |
	When import moves file
	Then I will receive this core error: InMoveWrong

Scenario: Eg22. Import with (Nature: In) (AccountOut:Yes) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: InMoveWrong

Scenario: Eg23. Import with (Nature: In) (AccountOut:Yes) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     | Account Out |    | 1     |
	When import moves file
	Then I will receive this core error: InMoveWrong

Scenario: Eg24. Import with (Nature: In) (AccountOut:No) (AccountIn:Unknown)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In      | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Unknown | 1     |
	When import moves file
	Then I will receive this core error: InvalidAccount

Scenario: Eg25. Import with (Nature: Transfer) (AccountOut:No) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer |             |            | 1     |
	When import moves file
	Then I will receive this core error: TransferMoveWrong

Scenario: Eg26. Import with (Nature: Transfer) (AccountOut:No) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer |     | Account In | 1     |
	When import moves file
	Then I will receive this core error: TransferMoveWrong

Scenario: Eg27. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:No)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out |    | 1     |
	When import moves file
	Then I will receive this core error: TransferMoveWrong

Scenario: Eg28. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Unknown)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In      | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Unknown | 1     |
	When import moves file
	Then I will receive this core error: InvalidAccount

Scenario: Eg29. Import with (Nature: Transfer) (AccountOut:Unknown) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out     | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Unknown | Account In | 1     |
	When import moves file
	Then I will receive this core error: InvalidAccount

Scenario: Eg30. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Equal to Out)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In          | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account Out | 1     |
	When import moves file
	Then I will receive this core error: CircularTransfer

Scenario: Eg31. Import without Value nor Details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       |
	When import moves file
	Then I will receive this core error: MoveValueOrDetailRequired

Scenario: Eg32. Import with Value zero and no Details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 0     |
	When import moves file
	Then I will receive this core error: MoveValueOrDetailRequired

Scenario: Eg33. Import without value and without Description in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       |              | 1       | 1      |
	When import moves file
	Then I will receive this core error: MoveDetailDescriptionRequired

Scenario: Eg34. Import without value and with Amount zero in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 0       | 1      |
	When import moves file
	Then I will receive this core error: MoveDetailAmountRequired

Scenario: Eg35. Import without value and with Value zero in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 0      |
	When import moves file
	Then I will receive this core error: MoveDetailValueRequired

Scenario: Eg36. Import with Description too large in Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1                                        | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxy | 1       | 1      |
	When import moves file
	Then I will receive this core error: TooLargeDetailDescription

Scenario: Eg37. Import with disabled Category
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: DisabledCategory

Scenario: Eg38. Import with closed AccountOut
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: ClosedAccount

Scenario: Eg39. Import with closed AccountIn
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive this core error: ClosedAccount

Scenario: Eg40. Import with info all right (Out)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out |    | 1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg41. Import with info all right (In)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Int    |     | Account In | 1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg42. Import with info all right (Transfer)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg45. Import negative (value)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | -1    |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg46. Import negative (details)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | -1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg47. Import with exactly length in Description of Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1                                       | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1       | 1      |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg48. Import with exactly length in Description
	Given a moves file with this content
			| Description                                        | Date       | Category | Nature   | Out         | In         | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg49. Import with details with same Description
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | Same         | 1       | 1      | Same         | 1       | 1      |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg50. Import with e-mail sender system out
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg51. Import with e-mail sender system ok
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |
		And these settings
			| SendMoveEmail | X     |
			| true          | false |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
		And no email will be sent

Scenario: Eg53. Import with decimals
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1.1   |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg54. Import with decimals in details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.1    |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg55. Import move transfer with unique value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     |            |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg56. Import move transfer with unique value for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1     |            |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion

Scenario: Eg57. Import move transfer with conversion for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In | 1     | 5          |
	When import moves file
	Then I will receive this core error: AccountsSameCurrencyConversion

Scenario: Eg58. Import move transfer with conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg59. Import move transfer with conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          |
		But these settings
			| UseCurrency |
			| false       |
	When import moves file
	Then I will receive this core error: UseCurrencyDisabled

Scenario: Eg60. Import move out with conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    | 1     | 5          |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer

Scenario: Eg61. Import move in with conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Int    |     | Account In BRL | 1     | 5          |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer

Scenario: Eg69. Import move transfer with conversion ZERO for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 0          |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion

Scenario: Eg71. Import move out with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    | 1     |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg72. Import move in with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL | 1     |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg62. Import move transfer with unique detailed value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1      |             |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg63. Import move transfer with unique detailed value for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      |             |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion

Scenario: Eg64. Import move transfer with detailed conversion for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1      | 5           |
	When import moves file
	Then I will receive this core error: AccountsSameCurrencyConversion

Scenario: Eg65. Import move transfer with detailed conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      | 5           |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg66. Import move transfer with detailed conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      | 5           |
		But these settings
			| UseCurrency |
			| false       |
	When import moves file
	Then I will receive this core error: UseCurrencyDisabled

Scenario: Eg67. Import move out with detailed conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    |     | Account In BRL |       | D            | 1       | 1      | 5           |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer

Scenario: Eg68. Import move in with detailed conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL |       | D            | 1       | 1      | 5           |
	When import moves file
	Then I will receive this core error: CurrencyInOutValueWithoutTransfer

Scenario: Eg70. Import move transfer with detailed conversion ZERO for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      | 0           |
	When import moves file
	Then I will receive this core error: AccountsDifferentCurrencyNoConversion

Scenario: Eg73. Import move out with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | Out    | Account Out EUR |    |       | D            | 1       | 1      |             |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded

Scenario: Eg74. Import move in with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-04-29 | Category | In     |     | Account In BRL |       | D            | 1       | 1      |             |
		And these settings
			| UseCurrency |
			| true        |
	When import moves file
	Then I will receive no core error
		And the pre-import data will be recorded
