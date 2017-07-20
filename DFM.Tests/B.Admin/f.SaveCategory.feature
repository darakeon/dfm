Feature: e. Creation of Category

Scenario: 01. Save Category without name (E)
	Given I have this category to create
		| Name |
		|      |
	When I try to save the category
	Then I will receive this error
		| Error                |
		| CategoryNameRequired |
	And the category will not be saved

Scenario: 02. Save Category with name that already exists (E)
	Given I have this category to create
		| Name        |
		| CategoryDFM |
	And I already have created this category
	When I try to save the category
	Then I will receive this error
		| Error                 |
		| CategoryAlreadyExists |
	And the category will not be saved

Scenario: 99. Save Category with info all right (S)
	Given I have this category to create
		| Name        |
		| CategoryDFM |
	When I try to save the category
	Then I will receive no error
	And the category will be saved
