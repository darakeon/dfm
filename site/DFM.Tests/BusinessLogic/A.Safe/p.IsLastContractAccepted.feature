Feature: p. Is last Contract accepted
	Background: 
		Given I have a contract
		And I have an active user

Scenario: 01. Get accepted contract
	Given I have accepted the contract
	When I try to get the acceptance
	Then I will receive no core error
	And the contract status will be accepted

Scenario: 02. Get not accepted contract
	Given I create a new contract
	When I try to get the acceptance
	Then I will receive no core error
	And the contract status will be not accepted
