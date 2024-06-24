Feature: Gi. Make move from imported

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
		And I have a category
		And I enable the category Category

Scenario: Gi01. Unlogged user
	Given I have no logged user (logoff)
	When make move from imported
	Then I will receive this core error: Uninvited

Scenario: Gi02. Non robot user
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When make move from imported
	Then I will receive this core error: Uninvited

Scenario: Gi03. Nothing to import
	When robot user login
		And make move from imported
	Then I will receive no core error

Scenario: Gi04. Import with user marked for deletion
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But the user is marked for deletion
	When robot user login
		And make move from imported
	Then I will receive this core error: UserDeleted
		And the line status will change to Error
		And the lines will be dequeued

Scenario: Gi05. Import with user requested wipe
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But the user asked data wipe
	When robot user login
		And make move from imported
	Then I will receive this core error: UserAskedWipe
		And the line status will change to Error
		And the lines will be dequeued

Scenario: Gi06. Import without sign last contract
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But there is a new contract
	When robot user login
		And make move from imported
	Then I will receive this core error: NotSignedLastContract
		And the line status will change to Error
		And the lines will be dequeued

Scenario: Gi07. Import with 20 details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 | Description3 | Amount3 | Value3 | Description4 | Amount4 | Value4 | Description5 | Amount5 | Value5 | Description6 | Amount6 | Value6 | Description7 | Amount7 | Value7 | Description8 | Amount8 | Value8 | Description9 | Amount9 | Value9 | Description10 | Amount10 | Value10 | Description11 | Amount11 | Value11 | Description12 | Amount12 | Value12 | Description13 | Amount13 | Value13 | Description14 | Amount14 | Value14 | Description15 | Amount15 | Value15 | Description16 | Amount16 | Value16 | Description17 | Amount17 | Value17 | Description18 | Amount18 | Value18 | Description19 | Amount19 | Value19 | Description20 | Amount20 | Value20 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | D1           | 1       | 101    | D2           | 2       | 202    | D3           | 3       | 303    | D4           | 4       | 404    | D5           | 5       | 505    | D6           | 6       | 606    | D7           | 7       | 707    | D8           | 8       | 808    | D9           | 9       | 909    | D10           | 10       | 1010    | D11           | 11       | 1111    | D12           | 12       | 1212    | D13           | 13       | 1313    | D14           | 14       | 1414    | D15           | 15       | 1515    | D16           | 16       | 1616    | D17           | 17       | 1717    | D18           | 18       | 1818    | D19           | 19       | 1919    | D20           | 20       | 2020    |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -289870
		And the month-category-accountOut value will change in 289870
		And the year-category-accountOut value will change in 289870
		And the accountIn value will change in 289870
		And the month-category-accountIn value will change in 289870
		And the year-category-accountIn value will change in 289870

Scenario: Gi08. Import without Category but using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 |          | Transfer | Account Out | Account In | 1     |
		And these settings
			| UseCategories |
			| false         |
		And the moves file was imported
		But these settings
			| UseCategories |
			| true          |
	When robot user login
		And make move from imported
	Then I will receive this core error: InvalidCategory
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi09. Import with Category but not using Categories
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And these settings
			| UseCategories |
			| true          |
		And the moves file was imported
		But these settings
			| UseCategories |
			| false         |
	When robot user login
		And make move from imported
	Then I will receive this core error: CategoriesDisabled
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi10. Import with (Nature: Transfer) (AccountOut:Yes) (AccountIn:Deleted)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But the account Account In is deleted
	When robot user login
		And make move from imported
	Then I will receive this core error: InvalidAccount
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi11. Import with (Nature: Transfer) (AccountOut:Deleted) (AccountIn:Yes)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But the account Account Out is deleted
	When robot user login
		And make move from imported
	Then I will receive this core error: InvalidAccount
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi12. Import with disabled Category
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But I disable the category Category
	When robot user login
		And make move from imported
	Then I will receive this core error: DisabledCategory
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi13. Import with closed AccountOut
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But I close the account Account Out
	When robot user login
		And make move from imported
	Then I will receive this core error: ClosedAccount
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi14. Import with closed AccountIn
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But I close the account Account In
	When robot user login
		And make move from imported
	Then I will receive this core error: ClosedAccount
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi15. Import with info all right (Out)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out         | In | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Out    | Account Out |    | 1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1

Scenario: Gi16. Import with info all right (In)
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | In     |     | Account In | 1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi17. Import with info all right (Transfer)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi18. Import negative (value)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | -1    |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi19. Import negative (details)
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | D            | 1       | -1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi20. Import with exactly length in Description of Detail
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1                                       | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 1       | 1      |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi21. Import with exactly length in Description
	Given a moves file with this content
			| Description                                        | Date       | Category | Nature   | Out         | In         | Value |
			| ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwx | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi22. Import with details with same Description
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 | Description2 | Amount2 | Value2 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | Same         | 1       | 1      | Same         | 1       | 1      |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -2
		And the month-category-accountOut value will change in 2
		And the year-category-accountOut value will change in 2
		And the accountIn value will change in 2
		And the month-category-accountIn value will change in 2
		And the year-category-accountIn value will change in 2

Scenario: Gi23. Import with decimals
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1.1   |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1.1
		And the month-category-accountOut value will change in 1.1
		And the year-category-accountOut value will change in 1.1
		And the accountIn value will change in 1.1
		And the month-category-accountIn value will change in 1.1
		And the year-category-accountIn value will change in 1.1

Scenario: Gi24. Import with decimals in details
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1.1    |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1.1
		And the month-category-accountOut value will change in 1.1
		And the year-category-accountOut value will change in 1.1
		And the accountIn value will change in 1.1
		And the month-category-accountIn value will change in 1.1
		And the year-category-accountIn value will change in 1.1

Scenario: Gi25. Import move transfer with unique value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi26. Import move transfer with conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 5
		And the month-category-accountIn value will change in 5
		And the year-category-accountIn value will change in 5

Scenario: Gi27. Import move transfer with conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Conversion |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
		But these settings
			| UseCurrency |
			| false       |
	When robot user login
		And make move from imported
	Then I will receive this core error: UseCurrencyDisabled
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi28. Import move out with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | Out    | Account Out EUR |    | 1     |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1

Scenario: Gi29. Import move in with unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value |
			| Move {scenarioCode} | 2024-06-23 | Category | In     |     | Account In BRL | 1     |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi30. Import move transfer with unique detailed value for same currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out | Account In |       | D            | 1       | 1      |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1

Scenario: Gi31. Import move transfer with detailed conversion for different currencies
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      | 5           |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1
		And the accountIn value will change in 5
		And the month-category-accountIn value will change in 5
		And the year-category-accountIn value will change in 5

Scenario: Gi32. Import move transfer with detailed conversion for disabled use currency
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out             | In             | Value | Description1 | Amount1 | Value1 | Conversion1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Transfer | Account Out EUR | Account In BRL |       | D            | 1       | 1      | 5           |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
		But these settings
			| UseCurrency |
			| false       |
	When robot user login
		And make move from imported
	Then I will receive this core error: UseCurrencyDisabled
		And the line status will change to Error
		And the lines will be dequeued
	Given test user login
	Then the move will not be saved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the accountIn value will not change
		And the month-category-accountIn value will not change
		And the year-category-accountIn value will not change

Scenario: Gi33. Import move out with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out             | In | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | Out    | Account Out EUR |    |       | D            | 1       | 1      |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountOut value will change in -1
		And the month-category-accountOut value will change in 1
		And the year-category-accountOut value will change in 1

Scenario: Gi34. Import move in with detailed unique value for enabled conversion
	Given a moves file with this content
			| Description         | Date       | Category | Nature | Out | In             | Value | Description1 | Amount1 | Value1 |
			| Move {scenarioCode} | 2024-06-23 | Category | In     |     | Account In BRL |       | D            | 1       | 1      |
		And these settings
			| UseCurrency |
			| true        |
		And the moves file was imported
	When robot user login
		And make move from imported
	Then I will receive no core error
		And the line status will change to Success
		And the lines will be dequeued
	Given test user login
	Then the move will be saved
		And the accountIn value will change in 1
		And the month-category-accountIn value will change in 1
		And the year-category-accountIn value will change in 1
