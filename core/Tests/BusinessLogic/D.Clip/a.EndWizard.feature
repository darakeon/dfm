Feature: Da. End wizard

Background:
	Given test user login

Scenario: Da01. End wizard
	Given these settings
			| Wizard |
			| true   |
	When I end wizard
	Then I will receive no core error

Scenario: Da02. End Wizard logged out
	Given I have no logged user (logoff)
	When I end wizard
	Then I will receive this core error: Uninvited

Scenario: Da03. Not end wizard if user is marked for deletion
	Given the user is marked for deletion
	When I end wizard
	Then I will receive this core error: UserDeleted

Scenario: Da04. Not end wizard if user requested wipe
	Given the user asked data wipe
	When I end wizard
	Then I will receive this core error: UserAskedWipe

Scenario: Da05. Not end wizard if not signed last contract
	Given there is a new contract
	When I end wizard
	Then I will receive this core error: NotSignedLastContract
