Feature: Ag. Update e-mail

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have a ticket of this user

Scenario: Ag01. E-mail change with wrong current password
	Given I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | password_wrong   |
	When I try to change the e-mail
	Then I will receive this core error: WrongPassword
		And the e-mail will not be changed
		And the user will be activated

Scenario: Ag02. E-mail change with empty new e-mail
	Given I pass this new e-mail and password
			| New E-mail | Current Password |
			|            | pass_word        |
	When I try to change the e-mail
	Then I will receive this core error: UserEmailRequired
		And the e-mail will not be changed
		And the user will be activated

Scenario: Ag03. E-mail change with info all right
	Given I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |
	When I try to change the e-mail
	Then I will receive no core error
		And the e-mail will be changed
		And the user will not be activated

Scenario: Ag04. Not update if user is marked for deletion
	Given I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |
		But the user is marked for deletion
	When I try to change the e-mail
	Then I will receive this core error: UserDeleted

Scenario: Ag05. Not update if user requested wipe
	Given I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |
		But the user asked data wipe
	When I try to change the e-mail
	Then I will receive this core error: UserAskedWipe

Scenario: Ag06. Update E-mail without signing contract
	Given I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |
		But there is a new contract
	When I try to change the e-mail
	Then I will receive this core error: NotSignedLastContract

Scenario: Ag07. TFA activated but without code
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this new e-mail and password
			| New E-mail                          | Current Password |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |
	When I try to change the e-mail
	Then I will receive this core error: TFAWrongCode
		And the e-mail will not be changed
		And the user will be activated

Scenario: Ag08. TFA activated but with empty code
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this new e-mail and password
			| New E-mail                          | Current Password | TFA Code |
			| new_{scenarioCode}@dontflymoney.com | pass_word        |          |
	When I try to change the e-mail
	Then I will receive this core error: TFAWrongCode
		And the e-mail will not be changed
		And the user will be activated

Scenario: Ag09. TFA activated but with wrong code
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this new e-mail and password
			| New E-mail                          | Current Password | TFA Code |
			| new_{scenarioCode}@dontflymoney.com | pass_word        | 123456   |
	When I try to change the e-mail
	Then I will receive this core error: TFAWrongCode
		And the e-mail will not be changed
		And the user will be activated

Scenario: Ag10. TFA activated with right code
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this new e-mail and password
			| New E-mail                          | Current Password | TFA Code    |
			| new_{scenarioCode}@dontflymoney.com | pass_word        | {generated} |
	When I try to change the e-mail
	Then I will receive no core error
		And the e-mail will be changed
		And the user will not be activated

Scenario: Ag11. TFA activated too much invalid tfa code attempts
	Given I have this two-factor data
			| Secret | TFA Code    | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this new e-mail and password
			| New E-mail                          | Current Password | TFA Code |
			| new_{scenarioCode}@dontflymoney.com | pass_word        | 123456   |
	When I try to change the e-mail
		And I try to change the e-mail
		And I try to change the e-mail
		And I try to change the e-mail
		And I try to change the e-mail
		And I try to change the e-mail
		And I try to change the e-mail
	Then I will receive this core error: TFATooMuchAttempt
		And the e-mail will not be changed
		And the user will not be activated
