Feature: b. Get Account by Name

Background:
	Given I have an active user
	And I have an account

Scenario: 01. Try to get Account with wrong Name (E)
	Given I pass a name of account that doesn't exist
	When I try to get the account by its name
	Then I will receive this core error: InvalidAccount
	And I will receive no account

Scenario: 99. Get the Account by Name (S)
	Given I pass a valid account name
	When I try to get the account by its name
	Then I will receive no core error
	And I will receive the account