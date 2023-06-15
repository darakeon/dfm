Feature: Hi. Wipe CSV

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And the user was wiped once


Scenario: Hi01. Wipe CSV with invalid token
	Given I pass an invalid token
	When I try to wipe the file
	Then I will receive this core error: InvalidToken
		And the file will not be wiped

Scenario: Hi02. Wipe CSV with token of user verification
	Given I pass a valid UserVerification token
	When I try to wipe the file
	Then I will receive this core error: InvalidToken
		And the file will not be wiped

Scenario: Hi03. Wipe CSV with token of password reset
	Given I pass a valid PasswordReset token
	When I try to wipe the file
	Then I will receive this core error: InvalidToken
		And the file will not be wiped

Scenario: Hi04. Wipe CSV with token of unsubscribe move mail
	Given I pass a valid UnsubscribeMoveMail token
	When I try to wipe the file
	Then I will receive this core error: InvalidToken
		And the file will not be wiped

Scenario: Hi05. Wipe CSV with info all right
	Given I pass a valid DeleteCsvData token
	When I try to wipe the file
	Then I will receive no core error
		And the file will be wiped
		And the token will not be valid anymore

Scenario: Hi06. Wipe CSV with token already used
	Given I pass a valid DeleteCsvData token
	When I try to wipe the file
	Then I will receive no core error
	# Same token
	When I try to wipe the file
	Then I will receive this core error: InvalidToken

Scenario: Hi07. Wipe CSV with token expired
	Given I pass a valid DeleteCsvData token
		But the token expires
	When I try to wipe the file
	Then I will receive this core error: InvalidToken

Scenario: Hi08. Wipe CSV twice
	Given I pass a valid DeleteCsvData token
	When I try to wipe the file
	Then I will receive no core error
	# again
	Given I pass a valid DeleteCsvData token
	When I try to wipe the file
	Then I will receive this core error: CSVNotFound
