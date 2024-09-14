Feature: Id. Finish Archives

Background:
	Given test user login
		And these settings
			| UseCategories | UseCurrency |
			| true          | true        |
		And I have two accounts
		And I have a category

Scenario: Id01. Unlogged user
	Given I have no logged user (logoff)
	When finish imported archives
	Then I will receive this core error: Uninvited

Scenario: Id02. Non robot user
	When finish imported archives
	Then I will receive this core error: Uninvited

Scenario: Id03. Nothing to finish
	When robot user login
		And finish imported archives
	Then I will receive no core error

Scenario: Id04. Success archive
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | In       |             | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		And robot made move from imported
		And robot made move from imported
		But line 4 is Canceled
	When robot user login
		And finish imported archives
	Then I will receive no core error
		And the archive status will change to Success

Scenario: Id05. Failed archive
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | In       |             | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | In       |             | Account In | 1     |
		And the moves file was imported
		But I close the account Account Out
		And robot made move from imported
		And robot made move from imported
		And robot made move from imported
		But line 4 is Canceled
	When robot user login
		And finish imported archives
	Then I will receive no core error
		And the archive status will change to Error

Scenario: Id06. Half processed archive
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | In       |             | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		And robot made move from imported
		But line 4 is Canceled
	When robot user login
		And finish imported archives
	Then I will receive no core error
		And the archive status will change to Pending

Scenario: Id07. All lines canceled
	Given a moves file with this content
			| Description         | Date       | Category | Nature   | Out         | In         | Value |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Out      | Account Out |            | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | In       |             | Account In | 1     |
			| Move {scenarioCode} | 2024-07-02 | Category | Transfer | Account Out | Account In | 1     |
		And the moves file was imported
		But line 1 is Canceled
		And line 2 is Canceled
		And line 3 is Canceled
		And line 4 is Canceled
	When robot user login
		And finish imported archives
	Then I will receive no core error
		And the archive status will change to Canceled
