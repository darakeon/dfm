Feature: Ci. Create category

Background:
	Given test user login
		And these settings
			| UseCategories |
			| true          |

Scenario: Ci01. Save Category without name
	Given I have this category to create
			| Name |
			|      |
	When I try to save the category
	Then I will receive this core error: CategoryNameRequired
		And the category will not be saved

Scenario: Ci02. Save Category with name that already exists
	Given I have this category to create
			| Name          |
			| Category Bf02 |
		And I already have this category
			| Name          |
			| Category Bf02 |
	When I try to save the category
	Then I will receive this core error: CategoryAlreadyExists
	#And the category will not be changed

Scenario: Ci03. Save Category with too big name
	Given I have this category to create
			| Name                  |
			| ABCDEFGHIJKLMNOPQRSTU |
	When I try to save the category
	Then I will receive this core error: TooLargeCategoryName
		And the category will not be saved

Scenario: Ci04. Save Category with exactly length name
	Given I have this category to create
			| Name                 |
			| ABCDEFGHIJKLMNOPQRST |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved

Scenario: Ci05. Save Category with info all right
	Given I have this category to create
			| Name          |
			| Category Bf05 |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved

Scenario: Ci06. Save Category with same name in another user
	Given I have this category to create
			| Name          |
			| Category Bf06 |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved
	Given there is another person logged in
		And these settings
			| UseCategories |
			| true          |
		And I have this category to create
			| Name          |
			| Category Bf06 |
	When I try to save the category
	Then I will receive no core error
		And the category will be saved

Scenario: Ci07. Not save Category if user is marked for deletion
	Given I have this category to create
			| Name          |
			| Category Bf07 |
		But the user is marked for deletion
	When I try to save the category
	Then I will receive this core error: UserDeleted

Scenario: Ci08. Not save Category if user requested wipe
	Given I have this category to create
			| Name          |
			| Category Bf08 |
		But the user asked data wipe
	When I try to save the category
	Then I will receive this core error: UserAskedWipe

Scenario: Ci09. Not save Category without signing contract
	Given I have this category to create
			| Name          |
			| Category Bf09 |
		But there is a new contract
	When I try to save the category
	Then I will receive this core error: NotSignedLastContract
