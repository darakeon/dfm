Feature: Ck. Update category

Background:
	Given test user login
		And I enable Categories use

Scenario: Ck01. Change the name
	Given I have this category
			| Name          |
			| Category Bh01 |
		And I make this changes to the category
			| Name            |
			| Bh01 - new name |
	When I try to update the category
	Then I will receive no core error
		And the category will be changed

Scenario: Ck02. Change the name to repeated
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

Scenario: Ck03. Change the name to empty
	Given I have this category
			| Name          |
			| Category Bh03 |
		And I make this changes to the category
			| Name |
			|      |
	When I try to update the category
	Then I will receive this core error: CategoryNameRequired
		And the category will not be changed

Scenario: Ck04. Change the name of another user category
	Given I have this category
			| Name          |
			| Category Bh04 |
		But there is a bad person logged in
			And I enable Categories use
		And I make this changes to the category
			| Name            |
			| Bh04 - new name |
	When I try to update the category
	Then I will receive this core error: InvalidCategory

Scenario: Ck05. Not update if user is marked for deletion
	Given I have this category
			| Name          |
			| Category Bh05 |
		And I make this changes to the category
			| Name            |
			| Bh05 - new name |
		But the user is marked for deletion
	When I try to update the category
	Then I will receive this core error: UserDeleted

Scenario: Ck06. Not update if user requested wipe
	Given I have this category
			| Name          |
			| Category Bh06 |
		And I make this changes to the category
			| Name            |
			| Bh06 - new name |
		But the user asked data wipe
	When I try to update the category
	Then I will receive this core error: UserAskedWipe

Scenario: Ck07. Not update without signing contract
	Given I have this category
			| Name          |
			| Category Bh07 |
		And I make this changes to the category
			| Name            |
			| Bh07 - new name |
		But there is a new contract
	When I try to update the category
	Then I will receive this core error: NotSignedLastContract
