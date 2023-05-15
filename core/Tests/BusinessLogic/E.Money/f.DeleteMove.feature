Feature: Ef. Delete move

Background:
	Given test user login
		And I enable Categories use
		And I have two accounts
		And I have a category

Scenario: Ef01. Try to delete Move with wrong ID
	Given I have a move with value 10 (Out)
		And I pass an id of Move that doesn't exist
	When I try to delete the move
	Then I will receive this core error: InvalidMove
		And the accountOut value will not change

Scenario: Ef02. Delete the Move Out by ID
	Given I have a move with value 10 (Out)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountOut value will change in 10

Scenario: Ef03. Delete the Move In by ID
	Given I have a move with value 10 (In)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountIn value will change in -10

Scenario: Ef04. Delete the Move Transfer by ID
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move
	Then I will receive no core error
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10

Scenario: Ef05. Delete with e-mail sender system out
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move with e-mail system out
	Then I will receive no core error
		And I will receive the notification
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10

Scenario: Ef06. Delete with e-mail sender system ok
	Given I have a move with value 10 (Transfer)
		And I pass valid Move ID
	When I try to delete the move with e-mail system ok
	Then I will receive no core error
		And I will receive no notification
		And the move will be deleted
		And the accountOut value will change in 10
		And the accountIn value will change in -10
		And the move e-mail will have an unsubscribe link

Scenario: Ef07. Delete move from schedule with Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9C   | 2014-03-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I enable Categories use
		And I save the schedule
		And robot run the scheduler and get the move
		And I disable Categories use
	When I try to delete the move
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ef08. Delete move from schedule without Category
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9D   | 2014-03-23 | Out    | 10    | 1     | False     | Monthly   | False           |
		And it has no Details
		And it has no Category
		And it has an Account Out
		And it has no Account In
		And I disable Categories use
		And I save the schedule
		And robot run the scheduler and get the move
		And I enable Categories use
	When I try to delete the move
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ef09. Delete all moves from schedule
	Given I have this schedule to create
			| Description | Date       | Nature | Value | Times | Boundless | Frequency | ShowInstallment |
			| Move Ce9E   | 2014-09-28 | Out    | 10    | 3     | False     | Monthly   | False           |
		And it has no Details
		And it has a Category
		And it has an Account Out
		And it has no Account In
		And I save the schedule
		And robot run the scheduler and get all the moves
	When I try to delete all the moves
	Then I will receive no core error
		And the accountOut value will not change

Scenario: Ef10. Delete another user's Move
	Given I have a move
		And I pass valid Move ID
		But there is a bad person logged in
	When I try to delete the move
	Then I will receive this core error: InvalidMove
	Given test user login
	Then the move will not be deleted
		And the accountOut value will not change

Scenario: Ef11. Not delete if user is marked for deletion
	Given I have a move
		And I pass valid Move ID
		But the user is marked for deletion
	When I try to delete the move
	Then I will receive this core error: UserDeleted

Scenario: Ef12. Not delete if user requested wipe
	Given I have a move
		And I pass valid Move ID
		But the user asked data wipe
	When I try to delete the move
	Then I will receive this core error: UserAskedWipe

Scenario: Ef13. Not delete if not signed last contract
	Given I have a move
		And I pass valid Move ID
		But there is a new contract
	When I try to delete the move
	Then I will receive this core error: NotSignedLastContract
