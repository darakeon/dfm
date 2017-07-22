Feature: j. Enable Category

Background:
	Given I have an user
	And I have a category

Scenario: 01. Enable a Category that doesn't exist (E)
	Given I pass an id of Category that doesn't exist
	When I try to enable the category
	Then I will receive this error
		| Error     |
		| InvalidID |

Scenario: 02. Enable a Category already enabled (E)
	Given I enable a category
	And I pass its id to enable again
	When I try to enable the category
	Then I will receive this error
		| Error           |
		| EnabledCategory |

Scenario: 99. Enable a Category with info all right (S)
	Given I give an id of disabled category
	When I try to enable the category
	Then I will receive no error
	And the category will be enabled