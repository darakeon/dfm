﻿Feature: Hi. Retry line

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have a category

Scenario: Hi01. Unlogged user
	Given I have no logged user (logoff)
	When retry line
	Then I will receive this core error: Uninvited

Scenario: Hi02. User marked for deletion
	Given the user is marked for deletion
	When retry line
	Then I will receive this core error: UserDeleted

Scenario: Hi03. User requested wipe
	Given the user asked data wipe
	When retry line
	Then I will receive this core error: UserAskedWipe

Scenario: Hi04. Without sign last contract
	Given there is a new contract
	When retry line
	Then I will receive this core error: NotSignedLastContract

Scenario: Hi05. Invalid line
	When retry line
	Then I will receive this core error: LineNotFound

Scenario: Hi06. Line wrong user
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And I close the account Account Out
		And robot made move from imported
		And robot finish archives
		But there is a bad person logged in
	When retry line
	Then I will receive this core error: LineNotFound
		And the line will be Error
		And the archive will be Error
		And the scheduled time will not change

Scenario: Hi07. Line pending
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
	When retry line
	Then I will receive this core error: LineRetryOnlyErrorOutOfLimitCanceled
		And the line will be Pending
		And the archive will be Pending
		And the scheduled time will not change

Scenario: Hi08. Line success
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		And robot finish archives
		And test user login
	When retry line
	Then I will receive this core error: LineRetryOnlyErrorOutOfLimitCanceled
		And the line will be Success
		And the archive will be Success
		And the scheduled time will not change

Scenario: Hi09. Line error
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And I close the account Account Out
		And robot made move from imported
		And robot finish archives
		And test user login
	When retry line
	Then I will receive no core error
		And the line will be Pending
		And the archive will be Pending
		And the scheduled time will change

Scenario: Hi10. Line canceled
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
		And line 1 is Canceled
	When retry line
	Then I will receive no core error
		And the line will be Pending
		And the archive will be Pending
		And the scheduled time will change

Scenario: Hi11. Line out of limit
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
		And line 1 is OutOfLimit
	When retry line
	Then I will receive no core error
		And the line will be Pending
		And the archive will be Pending
		And the scheduled time will change
