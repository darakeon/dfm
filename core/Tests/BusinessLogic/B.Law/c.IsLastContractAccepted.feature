Feature: Bc. Is last contract accepted

Background:
	Given I have a contract

Scenario: Bc01. Get accepted contract
	Given test user login
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be accepted

Scenario: Bc02. Get not accepted contract
	Given test user login
		And there is a new contract
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be not accepted

Scenario: Bc03. Get accepted if previous is not accepted
	Given there is a new contract
	Given test user login
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be accepted

Scenario: Bc04. Not get accepted contract if user is marked for deletion
	Given test user login
		But the user is marked for deletion
	When I try to get the acceptance
	Then I will receive this core error: UserDeleted

Scenario: Bc05. Not get accepted contract if user requested wipe
	Given test user login
		But the user asked data wipe
	When I try to get the acceptance
	Then I will receive this core error: UserAskedWipe
