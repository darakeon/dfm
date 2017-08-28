Feature: h. Update of Category

Background:
	Given I have an active user
	And I enable Categories use

Scenario: 01. Change the name (S)
	Given I have this category
		| Name          |
		| Category Ha01 |
	When I make this changes to the category
		| Name            |
		| Ca01 - new name |
	And I try to update the category
	Then I will receive no core error
	And the category will be changed