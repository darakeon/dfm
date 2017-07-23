Feature: f. Get an user by its e-mail

* Change to select an user to its token

Background:
	Given I have an user

Scenario: 01. Select with email that doesn't exist (E)
	Given I pass an e-mail that doesn't exist
	When I try to get the user without password
	Then I will receive this core error: InvalidUser
	And I will receive no user

Scenario: 99. Select with info all right (S)
	Given I pass valid e-mail
	When I try to get the user without password
	Then I will receive no core error
	And I will receive the user