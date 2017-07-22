Feature: i. Disable of Category

Background:
	Given I have an user
	And I have a category

Scenario: 01. Disable a Category that doesn't exist (E)
	Given I pass an id of Category that doesn't exist
	When I try to disable the category
	Then I will receive this error
		| Error     |
		| InvalidID |

Scenario: 02. Disable a Category already disabled (E)
	Given I disable a category
	And I pass its id to disable again
	When I try to disable the category
	Then I will receive this error
		| Error            |
		| DisabledCategory |

Scenario: 99. Disable a Category with info all right (S)
	Given I give an id of an enabled category
	When I try to disable the category
	Then I will receive no error
	And the category will be disabled