﻿Feature: Aj. Validate ticket two factor authentication

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

Scenario: Aj01. Validate with disabled two-factor
	Given I remove two-factor
		And I have this two-factor data
			| TFA Code |
			| any      |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFANotConfigured
		And the ticket will be valid

Scenario: Aj02. Validate with invalid code
	Given I have this two-factor data
			| TFA Code |
			| wrong    |
	When I try to validate the ticket two factor
	Then I will receive this core error: TFAWrongCode
		And the ticket will not be valid

Scenario: Aj03. Validate with valid code
	Given I have this two-factor data
			| Secret | TFA Code    |
			| 123    | {generated} |
	When I try to validate the ticket two factor
	Then I will receive no core error
		And the ticket will be valid

Scenario: Aj04. Not validate if user is marked for deletion
	Given I have this two-factor data
			| Secret | TFA Code    |
			| 123    | {generated} |
		But the user is marked for deletion
	When I try to validate the ticket two factor
	Then I will receive this core error: UserDeleted

Scenario: Aj05. Not validate if user requested wipe
	Given I have this two-factor data
			| Secret | TFA Code    |
			| 123    | {generated} |
		But the user asked data wipe
	When I try to validate the ticket two factor
	Then I will receive this core error: UserAskedWipe

Scenario: Aj06. Too much invalid tfa code attempts
	Given I have this two-factor data
			| TFA Code |
			| wrong    |
	When I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
	Then I will receive this core error: TFATooMuchAttempt
		And the ticket will not be valid
		And the user will not be activated

Scenario: Aj07. Reset limit on right attempt
	Given I have this two-factor data
			| TFA Code |
			| wrong    |
	When I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
		And I try to validate the ticket two factor
	Then I will receive this core error: TFAWrongCode
		And the ticket will not be valid
	Given I have this two-factor data
			| TFA Code    |
			| {generated} |
	When I try to validate the ticket two factor
	Then I will receive no core error
		And the ticket will be valid
		And the tfa wrong attempts will be 0
