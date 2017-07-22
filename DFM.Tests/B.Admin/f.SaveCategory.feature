Feature: f. Creation of Category

Background:
	Given I have an user

Scenario: 01. Save Category without name (E)
	Given I have this category to create
		| Name |
		|      |
	When I try to save the category
	Then I will receive this error: CategoryNameRequired
	And the category will not be saved

Scenario: 02. Save Category with name that already exists (E)
	Given I have this category to create
		| Name          |
		| Category Bf02 |
	And I already have this category
		| Name          |
		| Category Bf02 |
	When I try to save the category
	Then I will receive this error: CategoryAlreadyExists
	#And the category will not be changed

Scenario: 99. Save Category with info all right (S)
	Given I have this category to create
		| Name          |
		| Category Bf99 |
	When I try to save the category
	Then I will receive no error
	And the category will be saved
