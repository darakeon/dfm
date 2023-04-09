Feature: Operations with not signed contract

Background:
	Given test user login
		And I enable Categories use

Scenario: Zz34. Get Month Report
	Given there is a new contract
	When I try to get the month report
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz35. Get Year Report
	Given there is a new contract
	When I try to get the year report
	Then I will receive this core error: NotSignedLastContract

Scenario: Zz36. Search by Description
	Given there is a new contract
	When I try to search by description Something
	Then I will receive this core error: NotSignedLastContract
