Feature: Bh. Update category

Background:
	Given test user login
		And I enable Categories use

Scenario: Bh01. Change the name
	Given I have this category
			| Name          |
			| Category Bh01 |
		And I make this changes to the category
			| Name            |
			| Bh01 - new name |
	When I try to update the category
	Then I will receive no core error
		And the category will be changed

Scenario: Bh02. Change the name to repeated
	Given I have this category
			| Name            |
			| Category Bh02.1 |
		And I have this category
			| Name            |
			| Category Bh02.2 |
		And I make this changes to the category
			| Name            |
			| Category Bh02.1 |
	When I try to update the category
	Then I will receive this core error: CategoryAlreadyExists
		And the category will not be changed

Scenario: Bh03. Change the name to empty
	Given I have this category
			| Name          |
			| Category Bh03 |
		And I make this changes to the category
			| Name |
			|      |
	When I try to update the category
	Then I will receive this core error: CategoryNameRequired
		And the category will not be changed

Scenario: Bh04. Change the name of another user category
	Given I have this category
			| Name          |
			| Category Bh01 |
		But there is a bad person logged in
			And I enable Categories use
		And I make this changes to the category
			| Name            |
			| Bh04 - new name |
	When I try to update the category
	Then I will receive this core error: InvalidCategory
