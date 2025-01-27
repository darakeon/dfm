﻿Feature: Ai. Remove two factor authentication

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |

Scenario: Ai01. With wrong password
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| wrong    |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ai02. With all info right
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password  |
			| pass_word |
	When I try to remove two-factor
	Then I will receive no core error
		And the two-factor will be empty

Scenario: Ai03. With empty password
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password |
			|          |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ai04. With null password
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| {null}   |
	When I try to remove two-factor
	Then I will receive this core error: TFAWrongPassword
		And the two-factor will be [123]

Scenario: Ai05. Not remove if user is marked for deletion
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| password |
		But the user is marked for deletion
	When I try to remove two-factor
	Then I will receive this core error: UserDeleted

Scenario: Ai06. Not remove if user requested wipe
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password |
			| password |
		But the user asked data wipe
	When I try to remove two-factor
	Then I will receive this core error: UserAskedWipe

Scenario: Ai07. Not remove if not signed last contract
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have this two-factor data
			| Password  |
			| pass_word |
		But there is a new contract
	When I try to remove two-factor
	Then I will receive this core error: NotSignedLastContract
