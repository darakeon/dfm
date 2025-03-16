Feature: Gh. Send Wiped User CSV

Background:
	Given I have this user created
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |

Scenario: Gh01. Email is not email
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}_dontflymoney.com | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh02. Non existent user
	When ask wiped user csv
			| Email                              | Password  |
			| no_{scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh03. Still active user
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh04. Wrong password
	Given the user creation was 100 days before
		And the user have being warned twice
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password    |
			| {scenarioCode}@dontflymoney.com | no_password |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh05. User asked wipe
	Given data wipe was asked
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive this core error: WipeUserAsked
		And email with csv will not be sent

Scenario: Gh06. User without moves
	Given the user creation was 100 days before
		And the user have being warned twice
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive this core error: WipeNoMoves
		And email with csv will not be sent

Scenario: Gh07. User with moves
	Given the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive no core error
		And 1 email with csv will be sent with a link to delete it

Scenario: Gh08. Wiped twice
	Given the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
		# create again
		But I have this user created
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive no core error
		And 2 emails with csv will be sent with a link to delete it

Scenario: Gh09. Recover wiped file
	Given the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
		But I pass a valid DeleteCsvData token
			And I wipe the file
	When ask wiped user csv
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
	Then I will receive this core error: CSVNotFound
		And email with csv will not be sent

Scenario: Gh10. No username
	When ask wiped user csv
			| Email | Password  |
			|       | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh11. No password
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com |          |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh12. Short username
	When ask wiped user csv
			| Email              | Password  |
			| u@dontflymoney.com | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent

Scenario: Gh13. Short domain
	When ask wiped user csv
			| Email            | Password  |
			| {scenarioCode}@d | pass_word |
	Then I will receive this core error: WipeInvalid
		And email with csv will not be sent
