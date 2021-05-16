Feature: Ak. List logins

Scenario: Ak01. List all logins
	Given test user login
		And I login the user
		And I logoff the user
		And I login the user
	When I ask for current active logins
	Then I will receive no core error
		And they will be active
		And they will not have sensible information

Scenario: Ak02. Not list if user is marked for deletion
	Given test user login
		And I login the user
		But the user is marked for deletion
	When I ask for current active logins
	Then I will receive this core error: UserDeleted
