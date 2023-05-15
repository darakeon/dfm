Feature: Dg. ReMisc

Background:
	Given test user login
		And it has a Misc

Scenario: Dg01. Ask logged off
	Given I have no logged user (logoff)
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: Uninvited
		And the user will not have changed misc

Scenario: Dg02. Ask inactive user in allowed period
	Given the user not active after 6 days
	When pass a password that is right
		And regenerate misc
	Then I will receive no core error
		And the user will have changed misc

Scenario: Dg03. Ask inactive user after 7 days
	Given the user not active after 7 days
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: DisabledUser
		And the user will not have changed misc

Scenario: Dg04. Ask user marked for deletion
	Given data wipe was asked
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: UserAskedWipe
		And the user will not have changed misc

Scenario: Dg05. Ask with all right
	When pass a password that is right
		And regenerate misc
	Then I will receive no core error
		And the user will have changed misc

Scenario: Dg06. Ask with wrong password
	When pass a password that is not right
		And regenerate misc
	Then I will receive this core error: WrongPassword
		And the user will not have changed misc

Scenario: Dg07. Ask not signed last contract
	Given there is a new contract
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: NotSignedLastContract
		And the user will not have changed misc
