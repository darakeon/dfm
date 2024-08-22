Feature: Gp. Cancel Archive

Background:
	Given test user login
		And I have two accounts

Scenario: Gp01. Unlogged user
	Given I have no logged user (logoff)
	When cancel archive
	Then I will receive this core error: Uninvited

Scenario: Gp02. User marked for deletion
	Given the user is marked for deletion
	When cancel archive
	Then I will receive this core error: UserDeleted

Scenario: Gp03. User requested wipe
	Given the user asked data wipe
	When cancel archive
	Then I will receive this core error: UserAskedWipe

Scenario: Gp04. Without sign last contract
	Given there is a new contract
	When cancel archive
	Then I will receive this core error: NotSignedLastContract

Scenario: Gp05. Invalid archive
	When cancel archive
	Then I will receive this core error: ArchiveNotFound

Scenario: Gp06. Archive wrong user
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But there is a bad person logged in
	When cancel archive
	Then I will receive this core error: ArchiveNotFound
		And the archive will be Pending
		And the line will be Pending

Scenario: Gp07. Archive pending
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
	When cancel archive
	Then I will receive no core error
		And the archive will be Canceled
		And the line will be Canceled

Scenario: Gp08. Archive success
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		And robot finish archives
		And test user login
	When cancel archive
	Then I will receive this core error: ArchiveCancelNoSuccess
		And the archive will be Success
		And the line will be Success

Scenario: Gp09. Archive error
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And I close the account Account Out
		And robot made move from imported
		And robot finish archives
		And test user login
	When cancel archive
	Then I will receive no core error
		And the archive will be Canceled
		And the line will be Canceled

Scenario: Gp10. Archive canceled
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But line 1 is Canceled
		And robot finish archives
		And test user login
	When cancel archive
	Then I will receive no core error
		And the archive will be Canceled
		And the line will be Canceled

Scenario: Gp11. Archive mixed stati with one success
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		But test user login
		And I close the account Account Out
		And robot made move from imported
		And test user login
	When cancel archive
	Then I will receive no core error
		And the archive will be Success
		And the line 1 will be Success
		And the line 2 will be Canceled
		And the line 3 will be Canceled
