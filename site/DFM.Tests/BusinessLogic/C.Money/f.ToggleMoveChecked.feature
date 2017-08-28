Feature: f. Toggle Move Checked

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category

Scenario: 01. Mark a not checked move as checked
	Given I have a move with value 10 (Out)
	When I try to mark it as checked
	Then I will receive no core error
		And the move will be checked

Scenario: 02. Mark a checked move as not checked
	Given I have a move with value 10 (Out)
	When I try to mark it as not checked
	Then I will receive no core error
		And the move will not be checked
