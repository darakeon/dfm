Feature: Go. Cancel line

Background:
	Given test user login
		And I have two accounts

Scenario: Go01. Unlogged user
	Given I have no logged user (logoff)
	When cancel line
	Then I will receive this core error: Uninvited

Scenario: Go02. User marked for deletion
	Given the user is marked for deletion
	When cancel line
	Then I will receive this core error: UserDeleted

Scenario: Go03. User requested wipe
	Given the user asked data wipe
	When cancel line
	Then I will receive this core error: UserAskedWipe

Scenario: Go04. Without sign last contract
	Given there is a new contract
	When cancel line
	Then I will receive this core error: NotSignedLastContract

Scenario: Go05. Invalid line
	When cancel line
	Then I will receive this core error: LineNotFound

Scenario: Go06. Line wrong user
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But there is a bad person logged in
	When cancel line
	Then I will receive this core error: LineNotFound
		And the line will be Pending

Scenario: Go07. Line pending
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
	When cancel line
	Then I will receive no core error
		And the line will be Canceled

Scenario: Go08. Line success
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		And test user login
	When cancel line
	Then I will receive this core error: LineCancelNoSuccess
		And the line will be Success

Scenario: Go09. Line error
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And I close the account Account Out
		And robot made move from imported
		And test user login
	When cancel line
	Then I will receive no core error
		And the line will be Canceled

Scenario: Go10. Line canceled
	Given a moves file with this content
			| Description         | Date       | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-21 | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And test user login
		But line 1 is Canceled
	When cancel line
	Then I will receive no core error
		And the line will be Canceled
