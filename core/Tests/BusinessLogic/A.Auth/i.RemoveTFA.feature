Feature: Ai. Remove two factor authentication

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor

Scenario: Ai01. With all info right
	Given I have this two-factor data
			| TFA Code    | Password  |
			| {generated} | pass_word |
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty

Scenario: Ai02. With wrong password
	Given I have this two-factor data
			| TFA Code    | Password |
			| {generated} | wrong    |
	When I try to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]

Scenario: Ai03. With empty password
	Given I have this two-factor data
			| TFA Code    | Password |
			| {generated} |          |
	When I try to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]

Scenario: Ai04. With null password
	Given I have this two-factor data
			| TFA Code    | Password |
			| {generated} | {null}   |
	When I try to remove two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be [123]

Scenario: Ai05. With wrong code
	Given I have this two-factor data
			| TFA Code | Password  |
			| 150124   | pass_word |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be [123]

Scenario: Ai06. With empty code
	Given I have this two-factor data
			| TFA Code | Password  |
			|          | pass_word |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be [123]

Scenario: Ai07. With null code
	Given I have this two-factor data
			| TFA Code | Password  |
			| {null}   | pass_word |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be [123]

Scenario: Ai08. Not remove if user is marked for deletion
	Given I have this two-factor data
			| TFA Code    | Password |
			| {generated} | password |
		But the user is marked for deletion
	When I try to remove two-factor
	Then I will receive this core error: UserDeleted

Scenario: Ai09. Not remove if user requested wipe
	Given I have this two-factor data
			| TFA Code    | Password |
			| {generated} | password |
		But the user asked data wipe
	When I try to remove two-factor
	Then I will receive this core error: UserAskedWipe

Scenario: Ai10. Not remove if not signed last contract
	Given I have this two-factor data
			| TFA Code    | Password  |
			| {generated} | pass_word |
		But there is a new contract
	When I try to remove two-factor
	Then I will receive this core error: NotSignedLastContract

Scenario: Ai11. Remove if not configured
	Given I have this two-factor data
			| TFA Code    | Password  |
			| {generated} | pass_word |
		But I remove two-factor
	When I try to remove two-factor
	Then I will receive this core error: TFANotConfigured
		And the two-factor will be empty

Scenario: Ai12. Remove if set as password
	Given I have this two-factor data
			| TFA Code    | Password  |
			| {generated} | pass_word |
		And I set to use TFA as password
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty
		And the TFA can not be used as password

Scenario: Ai13. Too much invalid tfa code attempts
	Given I have this two-factor data
			| TFA Code | Password  |
			| 123456   | pass_word |
	When I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
	Then I will receive this core error: TFATooMuchAttempt
		And the two-factor will be [123]
		And the user will not be activated

Scenario: Ai14. Reset tfa limit on right attempt
	Given I have this two-factor data
			| TFA Code | Password  |
			| 123456   | pass_word |
	When I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
		And I try to remove two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be [123]
	Given I have this two-factor data
			| TFA Code    | Password  |
			| {generated} | pass_word |
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty
		And the tfa wrong attempts will be 0
