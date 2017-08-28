Feature: Bh. Update of Category

Background:
	Given I have an active user
	And I enable Categories use

Scenario: Bh01. Change the name (S)
	Given I have this category
		| Name          |
		| Category Ha01 |
	When I make this changes to the category
		| Name            |
		| Ca01 - new name |
	And I try to update the category
	Then I will receive no core error
	And the category will be changed

Scenario: Bh02. Change the name to repeated (E)
	Given I have this category
		| Name            |
		| Category Ha02.1 |
	And I have this category
		| Name            |
		| Category Ha02.2 |
	When I make this changes to the category
		| Name            |
		| Category Ha02.1 |
	And I try to update the category
	Then I will receive this core error: CategoryAlreadyExists
	And the category will not be changed

Scenario: Bh03. Change the name to empty (E)
	Given I have this category
		| Name          |
		| Category Ha03 |
	When I make this changes to the category
		| Name |
		|      |
	And I try to update the category
	Then I will receive this core error: CategoryNameRequired
	And the category will not be changed
