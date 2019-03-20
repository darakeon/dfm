Feature: Ap. Is last Contract accepted

Background:
	Given I have a contract

Scenario: Ap01. Get accepted contract
	Given I have a complete user logged in
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be accepted

Scenario: Ap02. Get not accepted contract
	Given I have a complete user logged in
		And there is a new contract
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be not accepted

Scenario: Ap03. Get accepted if previous is not accepted
	Given there is a new contract
	Given I have a complete user logged in
	When I try to get the acceptance
	Then I will receive no core error
		And the contract status will be accepted
