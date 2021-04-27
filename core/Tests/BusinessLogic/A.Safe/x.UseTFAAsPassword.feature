Feature: Ax. Use TFA as Password

Background:
	Given I have this user created
			| Email                           | Password | Active |
			| {scenarioCode}@dontflymoney.com | password | true   |
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

Scenario: Ax01. Activate TFA as password
	When I set to use TFA as password
	Then I will receive no core error
		And the TFA can be used as password
		And the TFA will not be asked
		And I can still login using normal password
		And the TFA will be asked

Scenario: Ax02. Deactivate TFA as password
	When I set to use TFA as password
		But I set to not use TFA as password
	Then I will receive no core error
		And the TFA can not be used as password
		And the TFA will be asked
