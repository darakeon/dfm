Feature: c. Get Account by Name

Background:
	Given I have an user
	And I have an account

Scenario: 01. Try to get Account with wrong Name (E)
	Given I pass an Name of account that doesn't exist
	When I try to get the account by its Name
	Then I will receive this error: InvalidName
	And I will receive no account

Scenario: 99. Get the Account by Name (S)
	Given I pass valid account Name
	When I try to get the account by its Name
	Then I will receive no error
	And I will receive the account