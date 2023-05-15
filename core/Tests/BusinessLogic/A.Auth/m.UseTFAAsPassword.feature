Feature: Am. Use TFA as Password

Background:
	Given I have this user created
			| Email                           | Password | Active | Signed |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I login this user
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor

Scenario: Am01. Activate TFA as password
	When I set to use TFA as password
	Then I will receive no core error
		And the TFA can be used as password
		And the TFA will not be asked
		And I can still login using normal password
		And the TFA will be asked

Scenario: Am02. Deactivate TFA as password
	When I set to use TFA as password
		But I set to not use TFA as password
	Then I will receive no core error
		And the TFA can not be used as password
		And the TFA will be asked

Scenario: Am03. Not activate if user is marked for deletion
	Given the user is marked for deletion
	When I set to use TFA as password
	Then I will receive this core error: UserDeleted

Scenario: Am04. Not deactivate if user is marked for deletion
	Given the user is marked for deletion
	When I set to not use TFA as password
	Then I will receive this core error: UserDeleted

Scenario: Am05. Not activate if user requested wipe
	Given the user asked data wipe
	When I set to use TFA as password
	Then I will receive this core error: UserAskedWipe

Scenario: Am06. Not deactivate if user requested wipe
	Given the user asked data wipe
	When I set to not use TFA as password
	Then I will receive this core error: UserAskedWipe

Scenario: Am07. Not activate if not signed last contract
	Given there is a new contract
	When I set to use TFA as password
	Then I will receive this core error: NotSignedLastContract

Scenario: Am08. Not deactivate if not signed last contract
	Given there is a new contract
	When I set to not use TFA as password
	Then I will receive this core error: NotSignedLastContract
