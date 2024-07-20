Feature: Gk. Requeue Lines

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have a category

Scenario: Gk01. Unlogged user
	Given I have no logged user (logoff)
	When requeue lines
	Then I will receive this core error: Uninvited

Scenario: Gk02. Non robot user
	When requeue lines
	Then I will receive this core error: Uninvited

Scenario: Gk03. No lines
	When robot user login
		And requeue lines
	Then I will receive no core error

Scenario: Gk04. All pending within period
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will be queued
		But the scheduled time will not change

Scenario: Gk05. Only success lines
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		And the line is from before the queue period
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will not be queued
		And the scheduled time will not change

Scenario: Gk06. Only error lines
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But I close the account Account Out
		And robot made move from imported
		And the line is from before the queue period
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will not be queued
		And the scheduled time will not change

Scenario: Gk07. Requeue line
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And the line is from before the queue period
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will be queued
		And the scheduled time will change
