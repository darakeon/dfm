Feature: Bb. Get Account by Url

Background:
	Given I have a complete user logged in
		And I have an account

Scenario: Bb01. Try to get Account with wrong Url
	Given I pass an url of account that doesn't exist
	When I try to get the account by its url
	Then I will receive this core error: InvalidAccount
		And I will receive no account

Scenario: Bb02. Get the Account by Url
	Given I pass a valid account url
	When I try to get the account by its url
	Then I will receive no core error
		And I will receive the account
