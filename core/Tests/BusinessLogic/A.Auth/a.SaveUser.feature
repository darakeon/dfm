Feature: Aa. Save user

Scenario: Aa01. Save user with empty e-mail
	Given I have this user data
			| Email | Password | Retype Password | Language | Timezone  |
			|       | password | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserEmailRequired
		And the user will not be saved

Scenario: Aa02. Save user with empty password
	Given I have this user data
			| Email                           | Password | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com |          |                 | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserPasswordRequired
		And the user will not be saved

Scenario: Aa03. Save user with invalid e-mail
	Given I have this user data
			| Email          | Password | Retype Password | Language | Timezone  |
			| {scenarioCode} | password | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserEmailInvalid
		And the user will not be saved

Scenario: Aa04. Save user with repeated e-mail
	Given I have this user created
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have this user data
			| Email                           | Password | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | password | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserAlreadyExists
		And the user will not be changed

Scenario: Aa05. Save user wrong retype
	Given I have this user data
			| Email                           | Password | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | password | password_wrong  | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: RetypeWrong
		And the user will not be saved

Scenario: Aa06. Save user with info all right
	Given I have this user data
			| Email                           | Password | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | password | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive no core error
		And the user will be saved
		And it will have a misc

Scenario: Aa07. Save user without e-mail
	Given I have this user data
			| Password | Retype Password | Language | Timezone  |
			| password | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserEmailRequired
		And the user will not be saved

Scenario: Aa08. Save user without password
	Given I have this user data
			| Email                           | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | password        | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: UserPasswordRequired
		And the user will not be saved

Scenario: Aa09. Save user without retype password
	Given I have this user data
			| Email                           | Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | password | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: RetypeWrong
		And the user will not be saved

Scenario: Aa10. Too large e-mail username (65)
	Given I have this user data
			| Password | Retype Password | Email                                                                              | Language | Timezone  |
			| password | password        | ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLM@dontflymoney.com | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: TooLargeUserEmail
		And the user will not be saved

Scenario: Aa11. Too large e-mail domain (256)
	Given I have this user data
			| Password | Retype Password | Email                                                                                                                                                                                                                                                                                       | Language | Timezone  |
			| password | password        | ABCDEFGHIJKLMNOPQRSTUVWXYZ@dontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.com | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: TooLargeUserEmail
		And the user will not be saved

Scenario: Aa12. Exactly length username (64) and domain (255)
	Given I have this user data
			| Password | Retype Password | Email                                                                                                                                                                                            | Language | Timezone  |
			| password | password        | ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKL@dontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.comdontflymoney.co | pt-BR    | UTC-03:00 |
	When I try to save the user
	Then I will receive no core error
		And the user will be saved
		And it will have a misc

Scenario: Aa13. Save user with invalid language
	Given I have this user data
			| Email                           | Password | Retype Password | Language | Timezone  |
			| {scenarioCode}@dontflymoney.com | P        | P               | at-ST    | UTC-03:00 |
	When I try to save the user
	Then I will receive this core error: LanguageUnknown
		And the user will not be saved

Scenario: Aa14. Save user with invalid timezone
	Given I have this user data
			| Email                           | Password | Retype Password | Language | Timezone |
			| {scenarioCode}@dontflymoney.com | P        | P               | pt-BR    | GMT-03   |
	When I try to save the user
	Then I will receive this core error: TimeZoneUnknown
		And the user will not be saved
