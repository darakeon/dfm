Feature: Gl. Get Archive List

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have a category

Scenario: Gl01. Unlogged user
	Given I have no logged user (logoff)
	When get archive list
	Then I will receive this core error: Uninvited

Scenario: Gl02. User marked for deletion
	Given the user is marked for deletion
	When get archive list
	Then I will receive this core error: UserDeleted

Scenario: Gl03. User requested wipe
	Given the user asked data wipe
	When get archive list
	Then I will receive this core error: UserAskedWipe

Scenario: Gl04. Without sign last contract
	Given there is a new contract
	When get archive list
	Then I will receive this core error: NotSignedLastContract

Scenario: Gl05. No archives
	When get archive list
	Then I will receive no core error
		And the archive list will be
			| Lines | Status  |

Scenario: Gl06. Get archives
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-20 | Category | Out      | Account Out |            | 1     |
		And the moves file was imported
		And robot made move from imported
		And robot made move from imported
		And test user login
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-20 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-20 | Category | In       |             | Account In | 1     |
		And the moves file was imported
		But I close the account Account In
		And robot made move from imported
		And robot made move from imported
		And robot made move from imported
		And test user login
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Out      | Account Out |            | 1     |
		And the moves file was imported
		And robot finish archives
		And test user login
	When get archive list
	Then I will receive no core error
		And the archive list will be
			| LineCount | Status  |
			| 1         | Pending |
			| 3         | Error   |
			| 2         | Success |
