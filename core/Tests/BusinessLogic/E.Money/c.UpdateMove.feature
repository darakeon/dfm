﻿Feature: Ec. Update move

Background:
	Given test user login
		And these settings
			| UseCategories | MoveCheck |
			| true          | true      |
		And I have two accounts
		And I have a category

Scenario: Ec01. Update the move date in 1 day
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		And I change the move date in -1 day
	When I update the move
	Then I will receive no core error
		And the move value will be 10
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		But the move is not checked for account Out

Scenario: Ec02. Update the move date in 1 month
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		And I change the move date in -1 month
	When I update the move
	Then I will receive no core error
		And the move value will be 10
		And the accountOut value will not change
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the year-category-accountOut value will not change
		But the move is not checked for account Out

Scenario: Ec03. Update the move date in 1 year
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		And I change the move date in -1 year
	When I update the move
	Then I will receive no core error
		And the move value will be 10
		And the accountOut value will not change
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the new-year-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in -10
		But the move is not checked for account Out

Scenario: Ec04. Update the move Category
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I change the category of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the accountOut value will not change
		And the month-accountOut value will not change
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the year-accountOut value will not change
		And the new-year-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in -10
		But the move is not checked for account Out

Scenario: Ec05. Update the move Account Out
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I change the account out of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the new-accountOut value will change in -10
		And the old-accountOut value will change in 10
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the new-year-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in -10
		But the move is not checked for account Out

Scenario: Ec06. Update the move Account In
	Given I have a move with value 10 (In)
		And the move is checked for account In
	When I change the account in of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the new-accountIn value will change in 10
		And the old-accountIn value will change in -10
		And the new-month-category-accountIn value will change in 10
		And the old-month-category-accountIn value will change in -10
		And the new-year-category-accountIn value will change in 10
		And the old-year-category-accountIn value will change in -10
		But the move is not checked for account In

Scenario: Ec07. Update the move Account Transfer (Out)
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I change the account out of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the new-accountOut value will change in -10
		And the old-accountOut value will change in 10
		And the new-month-category-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the new-year-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in -10
		But the move is not checked for account Out
		And the move is not checked for account In

Scenario: Ec08. Update the move Account Transfer (In)
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I change the account in of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the new-accountIn value will change in 10
		And the old-accountIn value will change in -10
		And the new-month-category-accountIn value will change in 10
		And the old-month-category-accountIn value will change in -10
		And the new-year-category-accountIn value will change in 10
		And the old-year-category-accountIn value will change in -10
		But the move is not checked for account Out
		And the move is not checked for account In

Scenario: Ec09. Update the move Account Transfer (Both)
	Given I have a move with value 10 (Transfer)
		And the move is checked for account Out
		And the move is checked for account In
	When I change the account out of the move
		And I change the account in of the move
		And I update the move
	Then I will receive no core error
		And the move value will be 10
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
		But the move is not checked for account Out
		And the move is not checked for account In

Scenario: Ec10. Update the move Out to In
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I change the move out to in
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the old-accountOut value will change in 10
		And the new-accountIn value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the new-month-category-accountIn value will change in 10
		And the old-year-category-accountOut value will change in -10
		And the new-year-category-accountIn value will change in 10
		But the move is not checked for account Out
		And the move is not checked for account In

Scenario: Ec11. Update the move In to Out
	Given I have a move with value 10 (In)
		And the move is checked for account In
	When I change the move in to out
		And I update the move
	Then I will receive no core error
		And the move value will be 10
		And the old-accountIn value will change in -10
		And the new-accountOut value will change in -10
		And the old-month-category-accountIn value will change in -10
		And the new-month-category-accountOut value will change in 10
		And the old-year-category-accountIn value will change in -10
		And the new-year-category-accountOut value will change in 10
		But the move is not checked for account In
		And the move is not checked for account Out

Scenario: Ec12. Update the move value
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I change the move value to 20
		And I update the move
	Then I will receive no core error
		And the move value will be 20
		And the old-accountOut value will change in -10
		And the old-month-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in 10
		But the move is not checked for account Out

Scenario: Ec13. Add details to the move
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb13a  | 1      | 10    |
		And the move is checked for account Out
	When I add these details to the move
			| Description | Amount | Value |
			| Move Cb13b  | 1      | 10    |
		And I update the move
	Then I will receive no core error
		And the move value will be 20
		And the old-accountOut value will change in -10
		And the old-month-category-accountOut value will change in 10
		And the old-year-category-accountOut value will change in 10
		But the move is not checked for account Out

Scenario: Ec14. Change the details of the move - remove and add
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb14a  | 1      | 10    |
		And the move is checked for account Out
	When I change the details of the move to
			| Description | Amount | Value |
			| Move Cb14b  | 2      | 20    |
		And I update the move
	Then I will receive no core error
		And the move value will be 40
		And the old-accountOut value will change in -30
		And the old-month-category-accountOut value will change in 30
		And the old-year-category-accountOut value will change in 30
		But the move is not checked for account Out

Scenario: Ec15. Change the details of the move - remove one
	Given I have a move with these details (Out)
			| Description | Amount | Value |
			| Move Cb15a  | 1      | 10    |
			| Move Cb15b  | 2      | 20    |
		And the move is checked for account Out
	When I change the details of the move to
			| Description | Amount | Value |
			| Move Cb15b  | 2      | 20    |
		And I update the move
	Then I will receive no core error
		And the move value will be 40
		And the old-accountOut value will change in 10
		And the old-month-category-accountOut value will change in -10
		And the old-year-category-accountOut value will change in -10
		But the move is not checked for account Out

Scenario: Ec16. Change move with schedule
	Given I have this schedule to create
			| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Cb16 | 2017-03-27 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And robot run the scheduler
		And I pass the first schedule move guid
		And I get the move
	When I update the move
	Then I will receive no core error
		And the Move will still be at the Schedule

Scenario: Ec17. Update the move of another user
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
		But there is a bad person logged in
		And these settings
			| UseCategories |
			| true          |
	When I change the move value to 20
		And I update the move
	Then I will receive this core error: MoveNotFound
	Given test user login
	Then the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the move is checked for account Out

Scenario: Ec18. Update the move adding details, but not removing value
	Given I have a move with value 10 (Out)
		And the move is checked for account Out
	When I add these details to the move
			| Description | Amount | Value |
			| Move Cb13b  | 1      | 20    |
		And I update the move
	Then I will receive this core error: MoveValueAndDetailNotAllowed
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change
		And the move is checked for account Out

Scenario: Ec19. Update the move value with e-mail system out
	Given I have a move with value 10 (Out)
		And these settings
			| SendMoveEmail |
			| true          |
		But email system is out
	When I update the move
	Then I will receive no core error
		And I will receive the notification

Scenario: Ec20. Update the move value with e-mail system ok
	Given I have a move with value 10 (Out)
		And these settings
			| SendMoveEmail |
			| true          |
	When I update the move
	Then I will receive no core error
		And I will receive no notification
		And the move e-mail will have an unsubscribe link

Scenario: Ec21. Update move date not changing its position at schedule
	Given I have this schedule to create
			| Description   | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Schedule Cb21 | 2020-11-01 | Out    | 10    | 5     | False     | Monthly   | True            |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And robot run the scheduler
		And I pass the first schedule move guid
		And I get the move
		And I change the move date in -2 months
	When I update the move
	Then the description will still be Schedule Cb21 [1/5]

Scenario: Ec22. Not update if user is marked for deletion
	Given I have a move with value 10 (Out)
		But the user is marked for deletion
	When I change the move value to 20
		And I update the move
	Then I will receive this core error: UserDeleted

Scenario: Ec23. Not update if user requested wipe
	Given I have a move with value 10 (Out)
		But the user asked data wipe
	When I change the move value to 20
		And I update the move
	Then I will receive this core error: UserAskedWipe

Scenario: Ec24. Not update if not signed last contract
	Given I have a move with value 10 (Out)
		But there is a new contract
	When I change the move value to 20
		And I update the move
	Then I will receive this core error: NotSignedLastContract

Scenario: Ec25. Update the move Account Transfer different Currency
	Given these settings
			| UseCurrency |
			| true        |
		And I have a move with value in 5 at account Poupanca (BRL) and out 1 at account Ordenado (EUR)
	When I change the account out of the move to Corrente (BRL)
		And I change the account in of the move to Recibos (EUR)
		And I update the move
	Then I will receive no core error
		And the new-accountOut value will change in -1
		And the old-accountOut value will change in 1
		And the new-month-category-accountOut value will change in 1
		And the old-month-category-accountOut value will change in -1
		And the new-year-category-accountOut value will change in 1
		And the old-year-category-accountOut value will change in -1
		And the new-accountIn value will change in 5
		And the old-accountIn value will change in -5
		And the new-month-category-accountIn value will change in 5
		And the old-month-category-accountIn value will change in -5
		And the new-year-category-accountIn value will change in 5
		And the old-year-category-accountIn value will change in -5

Scenario: Ec26. Add details to the move above limits
	Given these limits in user plan
			| MoveByAccountByMonth | DetailByParent |
			| 1                    | 3              |
		And I have a move with these details (Out)
			| Description | Amount | Value |
			| Detail 1    | 1      | 10    |
			| Detail 2    | 2      | 20    |
			| Detail 3    | 3      | 30    |
		And the move is checked for account Out
	When I add these details to the move
			| Description | Amount | Value |
			| Detail 4    | 4      | 40    |
		And I update the move
	Then I will receive this core error: PlanLimitDetailByParentAchieved
		And the accountOut value will not change
		And the month-category-accountOut value will not change
		And the year-category-accountOut value will not change

Scenario: Ec27. Update account out + month above limits
	Given these limits in user plan
			| MoveByAccountByMonth |
			| 3                    |
		And I have moves of
			| Description           | Date       | Nature | Value | 
			| Move {scenarioCode} 1 | 2024-09-13 | Out    | 1     |
			| Move {scenarioCode} 2 | 2024-09-13 | Out    | 2     |
			| Move {scenarioCode} 3 | 2024-09-13 | Out    | 3     |
		And I have a move with value 10 (Out)
		And I change the move date to 2024-09-13
	When I update the move
	Then I will receive this core error: PlanLimitMoveByAccountByMonthAchieved
		And the move will not be saved
		And the accountOut value will not change
		And the new-month-category-accountOut value will not change
		And the old-month-category-accountOut value will not change
		And the new-year-category-accountOut value will not change
		And the old-year-category-accountOut value will not change

Scenario: Ec28. Update account in + month above limits
	Given these limits in user plan
			| MoveByAccountByMonth |
			| 3                    |
		And I have moves of
			| Description           | Date       | Nature | Value | 
			| Move {scenarioCode} 1 | 2024-09-13 | In     | 1     |
			| Move {scenarioCode} 2 | 2024-09-13 | In     | 2     |
			| Move {scenarioCode} 3 | 2024-09-13 | In     | 3     |
		And I have a move with value 10 (In)
		And I change the move date to 2024-09-13
	When I update the move
	Then I will receive this core error: PlanLimitMoveByAccountByMonthAchieved
		And the move will not be saved
		And the accountIn value will not change
		And the new-month-category-accountIn value will not change
		And the old-month-category-accountIn value will not change
		And the new-year-category-accountIn value will not change
		And the old-year-category-accountIn value will not change
