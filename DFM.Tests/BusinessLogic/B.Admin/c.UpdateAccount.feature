Feature: c. Update of Account

Background:
	Given I have an active user

Scenario: 01. Change the name (S)
	Given I have this account
		| Name         | Yellow | Red |
		| Account Ca01 |        |     |
	When make this changes
		| Name            | Yellow | Red |
		| Ca01 - new name |        |     |
	And I try to update the account
	Then I will receive no core error
	And the account will be changed