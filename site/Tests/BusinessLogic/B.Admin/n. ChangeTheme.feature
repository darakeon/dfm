Feature: Bn. Change theme

Background:
	Given I have a complete user logged in

Scenario: Bn01. Change sistema Theme
	Given a theme Slate
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be Slate
