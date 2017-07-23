Feature: b. Update move

Background:
	Given I have an user
	And I have two accounts
	And I have a category
	And I have a move with value 10

Scenario: 01. Update the move date in 1 day
	Given I change the move date in -1 day
	When I update the move
	Then I will receive no error
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 02. Update the move date in 1 month
	Given I change the move date in -1 month
	When I update the move
	Then I will receive no error
	And the accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the year-category-accountOut value will not change

Scenario: 03. Update the move date in 1 year
	Given I change the move date in -1 year
	When I update the move
	Then I will receive no error
	And the accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 04. Update the move Category
	Given I change the category of the move
	When I update the move
	Then I will receive no error
	And the accountOut value will not change
	And the month-accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the year-accountOut value will not change
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 05. Update the move Account
	Given I change the account out of the move
	When I update the move
	Then I will receive no error
	And the new-accountOut value will change in -10
	And the old-accountOut value will change in 10
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10
