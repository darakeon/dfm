Feature: o. Accept Contract
	Background: 
		Given I have a contract
		And I have an active user

Scenario: 01. Accept contract not accepted before
	When I try to accept the contract
	Then I will receive no core error
	And the contract status will be accepted

Scenario: 02. Accept contract accepted before
	Given I have accepted the contract
	When I try to accept the contract
	Then I will receive no core error
	And the contract status will be accepted
