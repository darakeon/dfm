Feature: Am. Use TFA as Password

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor

Scenario: Am01. Activate TFA as password
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
	When I set to use TFA as password
	Then I will receive no core error
		And the TFA can be used as password
		And the TFA will not be asked
		And I can still login using normal password
		And the TFA will be asked

Scenario: Am02. Deactivate TFA as password
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
	When I set to not use TFA as password
	Then I will receive no core error
		And the TFA can not be used as password
		And the TFA will be asked

Scenario: Am03. Activate TFA as password with null code
	Given I have this two-factor data
			| Code   | Password  |
			| {null} | pass_word |
	When I set to use TFA as password
	Then I will receive this core error: TFAWrongCode
		And the TFA can not be used as password

Scenario: Am04. Deactivate TFA as password with null code
	Given I set to use TFA as password
		And I have this two-factor data
			| Code   | Password  |
			| {null} | pass_word |
	When I set to not use TFA as password
	Then I will receive this core error: TFAWrongCode
		And the TFA can be used as password

Scenario: Am05. Activate TFA as password with empty code
	Given I have this two-factor data
			| Code | Password  |
			|      | pass_word |
	When I set to use TFA as password
	Then I will receive this core error: TFAWrongCode
		And the TFA can not be used as password

Scenario: Am06. Deactivate TFA as password with empty code
	Given I set to use TFA as password
		And I have this two-factor data
			| Code | Password  |
			|      | pass_word |
	When I set to not use TFA as password
	Then I will receive this core error: TFAWrongCode
		And the TFA can be used as password

Scenario: Am07. Activate TFA as password with null password
	Given I have this two-factor data
			| Code        | Password |
			| {generated} | {null}   |
	When I set to use TFA as password
	Then I will receive this core error: WrongPassword
		And the TFA can not be used as password

Scenario: Am08. Deactivate TFA as password with null password
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password |
			| {generated} | {null}   |
	When I set to not use TFA as password
	Then I will receive this core error: WrongPassword
		And the TFA can be used as password

Scenario: Am09. Activate TFA as password with empty password
	Given I have this two-factor data
			| Code        | Password |
			| {generated} |          |
	When I set to use TFA as password
	Then I will receive this core error: WrongPassword
		And the TFA can not be used as password

Scenario: Am10. Deactivate TFA as password with empty password
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password |
			| {generated} |          |
	When I set to not use TFA as password
	Then I will receive this core error: WrongPassword
		And the TFA can be used as password

Scenario: Am11. Activate if user is marked for deletion
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But the user is marked for deletion
	When I set to use TFA as password
	Then I will receive this core error: UserDeleted

Scenario: Am12. Deactivate if user is marked for deletion
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But the user is marked for deletion
	When I set to not use TFA as password
	Then I will receive this core error: UserDeleted

Scenario: Am13. Activate if user requested wipe
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But the user asked data wipe
	When I set to use TFA as password
	Then I will receive this core error: UserAskedWipe

Scenario: Am14. Deactivate if user requested wipe
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But the user asked data wipe
	When I set to not use TFA as password
	Then I will receive this core error: UserAskedWipe

Scenario: Am15. Activate if not signed last contract
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But there is a new contract
	When I set to use TFA as password
	Then I will receive this core error: NotSignedLastContract
		And the TFA can not be used as password

Scenario: Am16. Deactivate if not signed last contract
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But there is a new contract
	When I set to not use TFA as password
	Then I will receive this core error: NotSignedLastContract
		And the TFA can be used as password

Scenario: Am17. Activate TFA as password with no TFA
	Given I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But I remove two-factor
	When I set to use TFA as password
	Then I will receive this core error: TFANotConfigured
		And the TFA can not be used as password

Scenario: Am18. Deactivate TFA as password with no TFA
	Given I set to use TFA as password
		And I have this two-factor data
			| Code        | Password  |
			| {generated} | pass_word |
		But I remove two-factor
	When I set to not use TFA as password
	Then I will receive this core error: TFANotConfigured
		And the TFA can not be used as password
