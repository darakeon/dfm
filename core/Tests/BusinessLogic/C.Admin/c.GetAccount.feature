Feature: Cc. Get account

Background:
	Given test user login
		And I have an account

Scenario: Cc01. Try to get Account with wrong Url
	Given I pass an url of account that doesn't exist
	When I try to get the account by its url
	Then I will receive this core error: InvalidAccount
		And I will receive no account

Scenario: Cc02. Get the Account by Url
	Given I pass a valid account url
	When I try to get the account by its url
	Then I will receive no core error
		And I will receive the account

Scenario: Cc03. Not get account if user is marked for deletion
	Given I pass a valid account url
		But the user is marked for deletion
	When I try to get the account by its url
	Then I will receive this core error: UserDeleted

Scenario: Cc04. Not get account if user requested wipe
	Given I pass a valid account url
		But the user asked data wipe
	When I try to get the account by its url
	Then I will receive this core error: UserAskedWipe

Scenario: Cc05. Not get account without signing contract
	Given I have an account
		And I pass a valid account url
		But there is a new contract
	When I try to get the account by its url
	Then I will receive this core error: NotSignedLastContract
