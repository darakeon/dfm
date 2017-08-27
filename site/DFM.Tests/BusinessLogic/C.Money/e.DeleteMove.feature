Feature: e. Delete of Moves

Background:
	Given I have an active user
	And I enable Categories use
	And I have two accounts
	And I have a category

Scenario: 01. Try to delete Move with wrong ID (E)
	Given I have a move with value 10 (Out)
	And I pass an id of Move that doesn't exist
	When I try to delete the move
	Then I will receive this core error: InvalidMove
	And the accountOut value will not change

Scenario: 97. Delete the Move Out by ID (S)
	Given I have a move with value 10 (Out)
	And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
	And the move will be deleted
	And the accountOut value will change in 10

Scenario: 98. Delete the Move In by ID (S)
	Given I have a move with value 10 (In)
	And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
	And the move will be deleted
	And the accountIn value will change in -10

Scenario: 99. Delete the Move Transfer by ID (S)
	Given I have a move with value 10 (Transfer)
	And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
	And the move will be deleted
	And the accountOut value will change in 10
	And the accountIn value will change in -10

Scenario: 9A. Delete with e-mail sender system out
	Given I have a move with value 10 (Transfer)
	And I pass valid Move ID
	When I try to delete the move with e-mail system out
	Then I will receive no core error
	And I will receive the notification
	And the move will be deleted
	And the accountOut value will change in 10
	And the accountIn value will change in -10

Scenario: 9B. Delete with e-mail sender system ok
	Given I have a move with value 10 (Transfer)
	And I pass valid Move ID
	When I try to delete the move with e-mail system ok
	Then I will receive no core error
	And I will receive no notification
	And the move will be deleted
	And the accountOut value will change in 10
	And the accountIn value will change in -10

Scenario: 9C. Delete move from schedule with Category
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Ce9C   | 2014-03-23 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 1     | False     | Monthly   | False           |
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

Scenario: 9D. Delete move from schedule without Category
	Given I have this future move to create
		| Description | Date       | Nature | Value |
		| Move Ce9D   | 2014-03-23 | Out    | 10    |
	And it has no Details
	And the move has this schedule
		| Times | Boundless | Frequency | ShowInstallment |
		| 1     | False     | Monthly   | False           |
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
