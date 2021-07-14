Feature: Az. ReMisc

Background:
	Given test user login
		And it has a Misc

Scenario: Az01. Ask logged off
	Given I have no logged user (logoff)
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: Uninvited
		And the user will not have changed misc

Scenario: Az02. Ask inactive user in allowed period
	Given the user not active after 6 days
	When pass a password that is right
		And regenerate misc
	Then I will receive no core error
		And the user will have changed misc

Scenario: Az03. Ask inactive user after 7 days
	Given the user not active after 7 days
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: DisabledUser
		And the user will not have changed misc

Scenario: Az04. Ask user marked for deletion
	Given data wipe was asked
	When pass a password that is right
		And regenerate misc
	Then I will receive this core error: UserAskedWipe
		And the user will not have changed misc

Scenario: Az05. Ask with all right
	When pass a password that is right
		And regenerate misc
	Then I will receive no core error
		And the user will have changed misc

Scenario: Az06. Ask with wrong password
	When pass a password that is not right
		And regenerate misc
	Then I will receive this core error: WrongPassword
		And the user will not have changed misc
