Feature: y. Ask data wipe

Background:
	Given test user login

Scenario: Ay01. Ask logged off
	Given I have no logged user (logoff)
	When ask data wipe
	Then I will receive this core error: Uninvited
		And the user will not be marked for deletion

Scenario: Ay02. Ask inactive user
	Given the user not active
	When ask data wipe
	Then I will receive this core error: DisabledUser
		And the user will not be marked for deletion

Scenario: Ay03. Ask user already marked for deletion
	Given data wipe was asked
	When ask data wipe
	Then I will receive no core error
		And the user will be marked for deletion

Scenario: Ay04. Ask with all right
	When ask data wipe
	Then I will receive no core error
		And the user will be marked for deletion
