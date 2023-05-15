Feature: Hh. Send Wiped User CSV

Background:
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |

Scenario: Hh01. Email is not email
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}_dontflymoney.com | password |
	Then I will receive this core error: WipeInvalid
		And the email with csv will not be sent

Scenario: Hh02. Non existent user
	When ask wiped user csv
			| Email                              | Password |
			| no_{scenarioCode}@dontflymoney.com | password |
	Then I will receive this core error: WipeInvalid
		And the email with csv will not be sent

Scenario: Hh03. Still active user
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	Then I will receive this core error: WipeInvalid
		And the email with csv will not be sent

Scenario: Hh04. Wrong password
	Given the user creation was 100 days before
		And the user have being warned twice
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password    |
			| {scenarioCode}@dontflymoney.com | no_password |
	Then I will receive this core error: WipeInvalid
		And the email with csv will not be sent

Scenario: Hh05. User asked wipe
	Given data wipe was asked
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	Then I will receive this core error: WipeUserAsked
		And the email with csv will not be sent

Scenario: Hh06. User without moves
	Given the user creation was 100 days before
		And the user have being warned twice
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	Then I will receive this core error: WipeNoMoves
		And the email with csv will not be sent

Scenario: Hh07. User with moves
	Given the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	Then I will receive no core error
		And the email with 1 csv will be sent

Scenario: Hh08. Wiped twice
	Given the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
		# create again
		But I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And the user creation was 100 days before
		And the user have being warned twice
		And the user have
			| System Stuff |
			| Move         |
		And robot call wipe users
	When ask wiped user csv
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
	Then I will receive no core error
		And the email with 2 csvs will be sent
