Feature: m. Get Category List

Background:
	Given I have an active user

Scenario: 01. Get all active categories
	Given I have this category
        | Name            |
        | Category Am01.1 |
	And I have this category
		| Name            |
		| Category Am01.2 |
	When ask for the active category list
	Then I will receive no core error
	And the category list will have this
		| Name            |
		| Category Am01.1 |
		| Category Am01.2 |

Scenario: 02. Get all active categories after close one
	Given I have this category
		| Name            |
		| Category Am02.1 |
	And I have this category
		| Name            |
		| Category Am02.2 |
	And I disable the category Category Am02.2
	When ask for the active category list
	Then I will receive no core error
	And the category list will have this
		| Name            |
		| Category Am02.1 |
	And the category list will not have this
		| Name            |
		| Category Am02.2 |

Scenario: 03. Get all not active categories after close one
	Given I have this category
		| Name            |
		| Category Am03.1 |
	And I have this category
		| Name            |
		| Category Am03.2 |
	And I disable the category Category Am03.2
	When ask for the not active category list
	Then I will receive no core error
	And the category list will have this
		| Name            |
		| Category Am03.2 |
	And the category list will not have this
		| Name            |
		| Category Am03.1 |

Scenario: 04. Get all categories after close one
	Given I have this category
		| Name            |
		| Category Am04.1 |
	And I have this category
		| Name            |
		| Category Am04.2 |
	And I disable the category Category Am04.2
	When ask for all the category list
	Then I will receive no core error
	And the category list will have this
		| Name            |
		| Category Am04.2 |
		| Category Am04.1 |
