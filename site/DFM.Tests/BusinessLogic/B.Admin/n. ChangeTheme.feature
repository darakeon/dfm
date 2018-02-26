Feature: Bn. Change Theme

Background:
	Given I have an active user
		And the user have accepted the contract

Scenario: Bn01. Change sistema Theme
	Given a theme Slate
	When I try to change the Theme
	Then I will receive no core error
		And the Theme will be Slate
