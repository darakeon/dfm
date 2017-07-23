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
	
Scenario: 02. Change the name when there is moves (S)
	Given I have this account
		| Name         | Yellow | Red |
		| Account Ca02 |        |     |
	And this account has moves
	When make this changes
		| Name            | Yellow | Red |
		| Ca02 - new name |        |     |
	And I try to update the account
	Then I will receive no core error
	And the account will be changed
	And the account value will not change