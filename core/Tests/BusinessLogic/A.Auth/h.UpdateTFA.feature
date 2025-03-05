Feature: Ah. Update two factor authentication

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |

Scenario: Ah01. With empty secret key
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			|        | {generated} | pass_word |
	When I try to set two-factor
	Then I will receive this core error: TFAEmptySecret
		And the two-factor will be empty

Scenario: Ah02. With wrong code
	Given I have this two-factor data
			| Secret | TFA Code | Password  |
			| 123    | wrong    | pass_word |
	When I try to set two-factor
	Then I will receive this core error: TFAWrongCode
		And the two-factor will be empty

Scenario: Ah03. With wrong password
	Given I have this two-factor data
			| Secret | TFA Code    | Password |
			| 123    | {generated} | wrong    |
	When I try to set two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be empty

Scenario: Ah04. With all info right
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [123]

Scenario: Ah05. Update two-factor
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 456    | {generated} | pass_word |
	When I try to set two-factor
	Then I will receive no core error
		And the two-factor will be [456]

Scenario: Ah06. With empty password
	Given I have this two-factor data
			| Secret | TFA Code    | Password |
			| 123    | {generated} |          |
	When I try to set two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be empty

Scenario: Ah07. With null password
	Given I have this two-factor data
			| Secret | TFA Code    | Password |
			| 123    | {generated} | {null}   |
	When I try to set two-factor
	Then I will receive this core error: WrongPassword
		And the two-factor will be empty

Scenario: Ah08. Not update if user is marked for deletion
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		But the user is marked for deletion
	When I try to set two-factor
	Then I will receive this core error: UserDeleted

Scenario: Ah09. Not update if user requested wipe
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		But the user asked data wipe
	When I try to set two-factor
	Then I will receive this core error: UserAskedWipe

Scenario: Ah10. Not update if not signed last contract
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		But there is a new contract
	When I try to set two-factor
	Then I will receive this core error: NotSignedLastContract

Scenario: Ah11. Remove tfa no warning after reactivate
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have a token for its tfa removal
		And I pass a valid RemoveTFA token
	When remove tfa by token
	Then I will receive no core error
		And the session will have a warning about tfa removed
	When I try to set two-factor
	Then I will receive no core error
		And the session will not have a warning about tfa removed
