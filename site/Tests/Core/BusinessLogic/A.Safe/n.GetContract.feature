Feature: An. Get contract
	Background:
		Given I have a contract

Scenario: An01. Get contract
	When I try to get the contract
	Then I will receive no core error
