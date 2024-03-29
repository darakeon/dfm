﻿Feature: Ae. List logins

Scenario: Ae01. List all logins
	Given test user login
		And I login the user
		And I login the user again
		And I logoff the user
		And I login the user
		And I login the user again
	When I ask for current active logins
	Then I will receive no core error
		And there will be 3 logins
		And they will be active
		And they will not have sensible information
		And the current login and only it will have current flag

Scenario: Ae02. Not list if user is marked for deletion
	Given test user login
		And I login the user
		But the user is marked for deletion
	When I ask for current active logins
	Then I will receive this core error: UserDeleted

Scenario: Ae03. Not list if user requested wipe
	Given test user login
		And I login the user
		But the user asked data wipe
	When I ask for current active logins
	Then I will receive this core error: UserAskedWipe

Scenario: Ae04. Uninvited if no user is logged in
	Given test user login
		And I logoff the user
	When I ask for current active logins
	Then I will receive this core error: Uninvited

Scenario: Ae05. List logins without signing contract
	Given test user login
		And I login the user
		And I logoff the user
		And I login the user
		But there is a new contract
	When I ask for current active logins
	Then I will receive this core error: NotSignedLastContract
