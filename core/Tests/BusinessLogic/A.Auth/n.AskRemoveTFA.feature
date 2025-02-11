Feature: An. Ask to remove two factor authentication

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor

Scenario: An01. With wrong password
	Given I have this two-factor data
			| Password |
			| wrong    |
	When I ask to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An02. With all info right
	Given I have this two-factor data
			| Password  |
			| pass_word |
	When I ask to remove two-factor
	Then I will receive no core error
		And the two-factor will be [123]
		And a token for remove TFA will be generated
		And email with a link to remove two-factor will be sent

Scenario: An03. With empty password
	Given I have this two-factor data
			| Password |
			|          |
	When I ask to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An04. With null password
	Given I have this two-factor data
			| Password |
			| {null}   |
	When I ask to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An05. Not remove if user is marked for deletion
	Given I have this two-factor data
			| Password |
			| password |
		But the user is marked for deletion
	When I ask to remove two-factor
	Then I will receive this core error: UserDeleted
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An06. Not remove if user requested wipe
	Given I have this two-factor data
			| Password |
			| password |
		But the user asked data wipe
	When I ask to remove two-factor
	Then I will receive this core error: UserAskedWipe
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An07. Not remove if not signed last contract
	Given I have this two-factor data
			| Password  |
			| pass_word |
		But there is a new contract
	When I ask to remove two-factor
	Then I will receive this core error: NotSignedLastContract
		And the two-factor will be [123]
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent

Scenario: An08. Ask remove with no TFA
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		And I remove two-factor
		But I have this two-factor data
			| Password  |
			| pass_word |
	When I ask to remove two-factor
	Then I will receive this core error: TFANotConfigured
		And the two-factor will be empty
		And a token for remove TFA will not be generated
		And email with a link to remove two-factor will not be sent
