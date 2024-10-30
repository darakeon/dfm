Feature: Ie. Requeue Lines

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have a category

Scenario: Ie01. Unlogged user
	Given I have no logged user (logoff)
	When requeue lines
	Then I will receive this core error: Uninvited

Scenario: Ie02. Non robot user
	When requeue lines
	Then I will receive this core error: Uninvited

Scenario: Ie03. No lines
	When robot user login
		And requeue lines
	Then I will receive no core error

Scenario: Ie04. All pending within period
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will be queued
		But the scheduled time will not change

Scenario: Ie05. Only success lines
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

Scenario: Ie06. Only error lines
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

Scenario: Ie07. Requeue line
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

Scenario: Ie08. No requeue canceled line
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And the line is from before the queue period
		But line 1 is Canceled
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will not be queued
		And the scheduled time will not change

Scenario: Ie09. No requeue out of limit line
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-20 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And the line is from before the queue period
		But line 1 is OutOfLimit
	When robot user login
		And requeue lines
	Then I will receive no core error
		And the lines will not be queued
		And the scheduled time will not change
