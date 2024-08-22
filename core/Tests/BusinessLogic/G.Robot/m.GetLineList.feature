Feature: Gm. Get Line List

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have these accounts
			| Name                 | Currency |
			| Account Out EUR      | EUR      |
			| Account In BRL       | BRL      |
		And I have a category

Scenario: Gm01. Unlogged user
	Given I have no logged user (logoff)
	When get line list
	Then I will receive this core error: Uninvited

Scenario: Gm02. User marked for deletion
	Given the user is marked for deletion
	When get line list
	Then I will receive this core error: UserDeleted

Scenario: Gm03. User requested wipe
	Given the user asked data wipe
	When get line list
	Then I will receive this core error: UserAskedWipe

Scenario: Gm04. Without sign last contract
	Given there is a new contract
	When get line list
	Then I will receive this core error: NotSignedLastContract

Scenario: Gm05. Invalid archive
	When get line list
	Then I will receive this core error: ArchiveNotFound

Scenario: Gm06. Archive wrong user
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-21 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-21 | Category | In       |             | Account In | 1     |
		And the moves file was imported
		But there is a bad person logged in
	When get line list
	Then I will receive this core error: ArchiveNotFound

Scenario: Gm07. Get lines
	Given a moves file with this content
			| Description           | Date       | Category | Nature   | Out             | In             | Value | Conversion | Description1  | Amount1 | Value1 | Conversion1 | Description2  | Amount2 | Value2 | Conversion2 | Description3  | Amount3 | Value3 | Conversion3 |
			| Move {scenarioCode} 1 | 2024-07-21 | Category | Transfer | Account Out EUR | Account In BRL |       |            | Description 1 | 1       | 1      | 5           | Description 2 | 2       | 2      | 6           | Description 3 | 3       | 3      | 7           |
			| Move {scenarioCode} 2 | 2024-07-21 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          |               |         |        |             |               |         |        |             |               |         |        |             |
			| Move {scenarioCode} 3 | 2024-07-21 | Category | Out      | Account Out     |                | 1     |            |               |         |        |             |               |         |        |             |               |         |        |             |
			| Move {scenarioCode} 4 | 2024-07-21 | Category | In       |                 | Account In     | 1     |            |               |         |        |             |               |         |        |             |               |         |        |             |
			| Move {scenarioCode} 5 | 2024-07-21 | Category | In       |                 | Account In     | 1     |            |               |         |        |             |               |         |        |             |               |         |        |             |
		And the moves file was imported
		And robot made move from imported
		And robot made move from imported
		But test user login
		And I close the account Account Out
		And robot made move from imported
		And line 5 is Canceled
		And test user login
	When get line list
	Then I will receive no core error
		And the line list will be
			| Position | Description           | Date       | Category | Nature   | Out             | In             | Value | Conversion | Status   | Description1  | Amount1 | Value1 | Conversion1 | Description2  | Amount2 | Value2 | Conversion2 | Description3  | Amount3 | Value3 | Conversion3 |
			| 1        | Move {scenarioCode} 1 | 2024-07-21 | Category | Transfer | Account Out EUR | Account In BRL |       |            | Success  | Description 1 | 1       | 1      | 5           | Description 2 | 2       | 2      | 6           | Description 3 | 3       | 3      | 7           |
			| 2        | Move {scenarioCode} 2 | 2024-07-21 | Category | Transfer | Account Out EUR | Account In BRL | 1     | 5          | Success  |               |         |        |             |               |         |        |             |               |         |        |             |
			| 3        | Move {scenarioCode} 3 | 2024-07-21 | Category | Out      | Account Out     |                | 1     |            | Error    |               |         |        |             |               |         |        |             |               |         |        |             |
			| 4        | Move {scenarioCode} 4 | 2024-07-21 | Category | In       |                 | Account In     | 1     |            | Pending  |               |         |        |             |               |         |        |             |               |         |        |             |
			| 5        | Move {scenarioCode} 5 | 2024-07-21 | Category | In       |                 | Account In     | 1     |            | Canceled |               |         |        |             |               |         |        |             |               |         |        |             |
