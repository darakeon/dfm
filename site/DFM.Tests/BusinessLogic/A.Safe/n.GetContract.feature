Feature: n. Get Contract
	Background: 
		Given I have a contract

Scenario: 01. Get contract
	When I try to get the contract
	Then I will receive no core error