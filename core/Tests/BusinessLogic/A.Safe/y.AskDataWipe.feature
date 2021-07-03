Feature: Ay. Ask data wipe

Background:
	Given test user login

Scenario: Ay01. Ask logged off
	Given I have no logged user (logoff)
	When pass a password that is right
		And ask data wipe
	Then I will receive this core error: Uninvited
		And the user will not be marked for deletion

Scenario: Ay02. Ask inactive user
	Given the user not active after 6 days
	When pass a password that is right
		And ask data wipe
	Then I will receive no core error
		And the user will be marked for deletion

Scenario: Ay03. Ask inactive user after 7 days
	Given the user not active after 7 days
	When pass a password that is right
		And ask data wipe
	Then I will receive this core error: DisabledUser
		And the user will not be marked for deletion

Scenario: Ay04. Ask user already marked for deletion
	Given data wipe was asked
	When pass a password that is right
		And ask data wipe
	Then I will receive this core error: UserAskedWipe

Scenario: Ay05. Ask with all right
	When pass a password that is right
		And ask data wipe
	Then I will receive no core error
		And the user will be marked for deletion

Scenario: Ay06. Ask with wrong password
	When pass a password that is not right
		And ask data wipe
	Then I will receive this core error: WrongPassword
		And the user will not be marked for deletion
