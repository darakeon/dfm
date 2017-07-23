Feature: b. Update move

Background:
	Given I have an user
	And I have two accounts
	And I have a category

Scenario: 01. Update the move date in 1 day
	Given I have a move with value 10 (Out)
	When I change the move date in -1 day
	And I update the move
	Then I will receive no core error
	And the accountOut value will not change
	And the month-category-accountOut value will not change
	And the year-category-accountOut value will not change

Scenario: 02. Update the move date in 1 month
	Given I have a move with value 10 (Out)
	When I change the move date in -1 month
	And I update the move
	Then I will receive no core error
	And the accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the year-category-accountOut value will not change

Scenario: 03. Update the move date in 1 year
	Given I have a move with value 10 (Out)
	When I change the move date in -1 year
	And I update the move
	Then I will receive no core error
	And the accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 04. Update the move Category
	Given I have a move with value 10 (Out)
	When I change the category of the move
	And I update the move
	Then I will receive no core error
	And the accountOut value will not change
	And the month-accountOut value will not change
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the year-accountOut value will not change
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 05. Update the move Account Out
	Given I have a move with value 10 (Out)
	When I change the account out of the move
	And I update the move
	Then I will receive no core error
	And the new-accountOut value will change in -10
	And the old-accountOut value will change in 10
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 06. Update the move Account In
	Given I have a move with value 10 (In)
	When I change the account in of the move
	And I update the move
	Then I will receive no core error
	And the new-accountIn value will change in 10
	And the old-accountIn value will change in -10
	And the new-month-category-accountIn value will change in 10
	And the old-month-category-accountIn value will change in -10
	And the new-year-category-accountIn value will change in 10
	And the old-year-category-accountIn value will change in -10


Scenario: 07. Update the move Account Transfer (Out)
	Given I have a move with value 10 (Transfer)
	When I change the account out of the move
	And I update the move
	Then I will receive no core error
	And the new-accountOut value will change in -10
	And the old-accountOut value will change in 10
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10


Scenario: 08. Update the move Account Transfer (In)
	Given I have a move with value 10 (Transfer)
	When I change the account in of the move
	And I update the move
	Then I will receive no core error
	And the new-accountIn value will change in 10
	And the old-accountIn value will change in -10
	And the new-month-category-accountIn value will change in 10
	And the old-month-category-accountIn value will change in -10
	And the new-year-category-accountIn value will change in 10
	And the old-year-category-accountIn value will change in -10


Scenario: 09. Update the move Account Transfer (Both)
	Given I have a move with value 10 (Transfer)
	When I change the account out of the move
	And I change the account in of the move
	And I update the move
	Then I will receive no core error
	And the new-accountOut value will change in -10
	And the old-accountOut value will change in 10
	And the new-month-category-accountOut value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-year-category-accountOut value will change in 10
	And the old-year-category-accountOut value will change in -10
	And the new-accountIn value will change in 10
	And the old-accountIn value will change in -10
	And the new-month-category-accountIn value will change in 10
	And the old-month-category-accountIn value will change in -10
	And the new-year-category-accountIn value will change in 10
	And the old-year-category-accountIn value will change in -10


Scenario: 10. Update the move Out to In
	Given I have a move with value 10 (Out)
	When I change the move out to in
	And I update the move
	Then I will receive no core error
	And the old-accountOut value will change in 10
	And the new-accountIn value will change in 10
	And the old-month-category-accountOut value will change in -10
	And the new-month-category-accountIn value will change in 10
	And the old-year-category-accountOut value will change in -10
	And the new-year-category-accountIn value will change in 10


Scenario: 11. Update the move In to Out
	Given I have a move with value 10 (In)
	When I change the move in to out
	And I update the move
	Then I will receive no core error
	And the old-accountIn value will change in -10
	And the new-accountOut value will change in -10
	And the old-month-category-accountIn value will change in -10
	And the new-month-category-accountOut value will change in 10
	And the old-year-category-accountIn value will change in -10
	And the new-year-category-accountOut value will change in 10
