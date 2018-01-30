Feature: Bm. Get Category List

Background:
	Given I have an active user who have accepted the contract

Scenario: Bm01. Get all active categories
	Given I have this category
        | Name            |
        | Category Bm01.1 |
		And I have this category
			| Name            |
			| Category Bm01.2 |
	When ask for the active category list
	Then I will receive no core error
		And the category list will have this
			| Name            |
			| Category Bm01.1 |
			| Category Bm01.2 |

Scenario: Bm02. Get all active categories after close one
	Given I have this category
			| Name            |
			| Category Bm02.1 |
		And I have this category
			| Name            |
			| Category Bm02.2 |
		And I disable the category Category Bm02.2
	When ask for the active category list
	Then I will receive no core error
		And the category list will have this
			| Name            |
			| Category Bm02.1 |
		And the category list will not have this
			| Name            |
			| Category Bm02.2 |

Scenario: Bm03. Get all not active categories after close one
	Given I have this category
			| Name            |
			| Category Bm03.1 |
		And I have this category
			| Name            |
			| Category Bm03.2 |
		And I disable the category Category Bm03.2
	When ask for the not active category list
	Then I will receive no core error
		And the category list will have this
			| Name            |
			| Category Bm03.2 |
		And the category list will not have this
			| Name            |
			| Category Bm03.1 |

Scenario: Bm04. Get all categories after close one
	Given I have this category
			| Name            |
			| Category Bm04.1 |
		And I have this category
			| Name            |
			| Category Bm04.2 |
		And I disable the category Category Bm04.2
	When ask for all the category list
	Then I will receive no core error
		And the category list will have this
			| Name            |
			| Category Bm04.2 |
			| Category Bm04.1 |
