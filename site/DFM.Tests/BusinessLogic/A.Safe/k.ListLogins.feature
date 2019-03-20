Feature: Ak. List logins

Scenario: Ak01. List all logins
	Given I have a complete user logged in
		And I login the user
		And I logoff the user
		And I login the user
	When I ask for current active logins
	Then I will receive no core error
		And they will be active
		And they will not have sensible information
