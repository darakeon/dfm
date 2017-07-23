Feature: c. Get Account

Background:
	Given I have an user
	And I have an account

Scenario: 01. Try to get Account with wrong ID (E)
	Given I pass an id of account that doesn't exist
	When I try to get the account
	Then I will receive this error: InvalidAccount
	And I will receive no account

Scenario: 99. Get the Account by ID (S)
	Given I pass valid account ID
	When I try to get the account
	Then I will receive no error
	And I will receive the account