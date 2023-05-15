Feature: Ba. Get contract
	Background:
		Given I have a contract

Scenario: Ba01. Get contract
	When I try to get the contract
	Then I will receive no core error
