Feature: Bf. Creation of Category

Background:
	Given I have an active user
	And I enable Categories use

Scenario: Bf01. Save Category without name (E)
	Given I have this category to create
		| Name |
		|      |
	When I try to save the category
	Then I will receive this core error: CategoryNameRequired
	And the category will not be saved

Scenario: Bf02. Save Category with name that already exists (E)
	Given I have this category to create
		| Name          |
		| Category Bf02 |
	And I already have this category
		| Name          |
		| Category Bf02 |
	When I try to save the category
	Then I will receive this core error: CategoryAlreadyExists
	#And the category will not be changed

Scenario: Bf03. Save Category with too big name (E)
	Given I have this category to create
		| Name                  |
		| ABCDEFGHIJKLMNOPQRSTU |
	When I try to save the category
	Then I will receive this core error: TooLargeData
	And the category will not be saved


Scenario: Bf98. Save Category with exactly length name (S)
	Given I have this category to create
		| Name                 |
		| ABCDEFGHIJKLMNOPQRST |
	When I try to save the category
	Then I will receive no core error
	And the category will be saved

Scenario: Bf99. Save Category with info all right (S)
	Given I have this category to create
		| Name          |
		| Category Bf99 |
	When I try to save the category
	Then I will receive no core error
	And the category will be saved
