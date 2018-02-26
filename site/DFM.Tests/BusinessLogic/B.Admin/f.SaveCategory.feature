Feature: Bf. Creation of Category

Background:
	Given I have an active user
		And the user have accepted the contract
		And I enable Categories use

Scenario: Bf01. Save Category without name
	Given I have this category to create
			| Name |
			|      |
	When I try to save the category
	Then I will receive this core error: CategoryNameRequired
		And the category will not be saved

Scenario: Bf02. Save Category with name that already exists
	Given I have this category to create
			| Name          |
			| Category Bf02 |
		And I already have this category
			| Name          |
			| Category Bf02 |
	When I try to save the category
	Then I will receive this core error: CategoryAlreadyExists
	#And the category will not be changed

Scenario: Bf03. Save Category with too big name
	Given I have this category to create
			| Name                  |
			| ABCDEFGHIJKLMNOPQRSTU |
	When I try to save the category
	Then I will receive this core error: TooLargeData
		And the category will not be saved

Scenario: Bf04. Save Category with exactly length name
	Given I have this category to create
			| Name                 |
			| ABCDEFGHIJKLMNOPQRST |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved

Scenario: Bf05. Save Category with info all right
	Given I have this category to create
			| Name          |
			| Category Bf99 |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved
