﻿Feature: Ce. Delete move

Background:
	Given I have a complete user logged in
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Ce01. Try to delete Move with wrong ID
	Given I have a move with value 10 (Out)
		And I pass an id of Move that doesn't exist
	When I try to delete the move
	Then I will receive this core error: InvalidMove
		And the accountOut value will not change

Scenario: Ce02. Delete the Move Out by ID
	Given I have a move with value 10 (Out)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountOut value will change in 10

Scenario: Ce03. Delete the Move In by ID
	Given I have a move with value 10 (In)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountIn value will change in -10

Scenario: Ce04. Delete the Move Transfer by ID
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10

Scenario: Ce05. Delete with e-mail sender system out
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move with e-mail system out
	Then I will receive no core error
		And I will receive the notification
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10

Scenario: Ce06. Delete with e-mail sender system ok
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move with e-mail system ok
	Then I will receive no core error
		And I will receive no notification
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10

Scenario: Ce07. Delete move from schedule with Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9C   | 2014-03-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I enable Categories use
		And I save the schedule
		And I run the scheduler and get the move
		And I disable Categories use
	When I try to delete the move
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ce08. Delete move from schedule without Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9D   | 2014-03-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
		And I disable Categories use
		And I save the schedule
		And I run the scheduler and get the move
		And I enable Categories use
	When I try to delete the move
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ce09. Delete all moves from schedule
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9E   | 2014-09-28 | Out    | 10    | 3     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And I run the scheduler and get all the moves
	When I try to delete all the moves
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ce10. Delete another user's Move
	Given I have a move
		And I pass valid Move ID
		But there is a bad person logged in
	When I try to delete the move
	Then I will receive this core error: InvalidMove
	Given the right user login again
	Then the move will not be deleted
		And the accountOut value will not change
