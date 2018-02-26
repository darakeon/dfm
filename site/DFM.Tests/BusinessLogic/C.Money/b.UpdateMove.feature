Feature: Cb. Update move

Background:
	Given I have an active user
		And the user have accepted the contract
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Cb01. Update the move date in 1 day
	Given I have a move with value 10 (Out)
		And I change the move date in -1 day
	When I update the move
	Then I will receive no core error
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Cb02. Update the move date in 1 month
	Given I have a move with value 10 (Out)
		And I change the move date in -1 month
	When I update the move
	Then I will receive no core error
		And the accountOut value will not change
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the year-category-accountOut value will not change

Scenario: Cb03. Update the move date in 1 year
	Given I have a move with value 10 (Out)
		And I change the move date in -1 year
	When I update the move
	Then I will receive no core error
		And the accountOut value will not change
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the new-year-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in -10

Scenario: Cb04. Update the move Category
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

Scenario: Cb05. Update the move Account Out
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

Scenario: Cb06. Update the move Account In
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

Scenario: Cb07. Update the move Account Transfer (Out)
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

Scenario: Cb08. Update the move Account Transfer (In)
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

Scenario: Cb09. Update the move Account Transfer (Both)
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

Scenario: Cb10. Update the move Out to In
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

Scenario: Cb11. Update the move In to Out
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

Scenario: Cb12. Update the move value
	Given I have a move with value 10 (Out)
	When I change the move value to 20
		And I update the move
	Then I will receive no core error
		And the old-accountOut value will change in -10
		And the old-month-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in 10

Scenario: Cb13. Add details to the move
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb13a  | 1      | 10    |
	When I add these details to the move
			| Description | Amount | Value |
			| Move Cb13b  | 1      | 10    |
		And I update the move
	Then I will receive no core error
		And the old-accountOut value will change in -10
		And the old-month-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in 10

Scenario: Cb14. Change the details of the move - remove and add
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb14a  | 1      | 10    |
	When I change the details of the move to
			| Description | Amount | Value |
			| Move Cb14b  | 2      | 20    |
		And I update the move
	Then I will receive no core error
		And the move total will be 40
		And the old-accountOut value will change in -30
		And the old-month-category-accountOut value will change in 30
		And the old-year-category-accountOut value will change in 30

Scenario: Cb15. Change the details of the move - remove one
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb15a  | 1      | 10    |
			| Move Cb15b  | 2      | 20    |
	When I change the details of the move to
			| Description | Amount | Value |
			| Move Cb15b  | 2      | 20    |
		And I update the move
	Then I will receive no core error
		And the move total will be 40
		And the old-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the old-year-category-accountOut value will change in -10

Scenario: Cb16. Change move with schedule
	Given I have this schedule to create
			| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Cb16 | 2017-03-27 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I run the scheduler
		And I get the Move at position 1 of the Schedule
	When I update the move
	Then I will receive no core error
		And the Move will still be at the Schedule
