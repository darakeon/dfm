Feature: Cd. Get Detail

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have two accounts
		And I have a category
		And I have a move with details

Scenario: Cd01. Try to get Detail with wrong ID
	Given I pass an id of Detail that doesn't exist
	When I try to get the detail
	Then I will receive this core error: InvalidDetail
		And I will receive no detail

Scenario: Cd02. Get the Detail by ID
	Given I pass valid Detail ID
	When I try to get the detail
	Then I will receive no core error
		And I will receive the detail

Scenario: Cd03. Get another user's Detail
	Given I pass valid Detail ID
		But there is a bad person logged in
	When I try to get the detail
	Then I will receive this core error: InvalidMove
		And I will receive no detail
