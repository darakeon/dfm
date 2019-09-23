Feature: Af. Get a session by its ticket

Background:
	Given I have this user created and activated
			| Email                               | Password | Retype Password |
			| selectuserbyticket@dontflymoney.com | password | password        |
		And I have a ticket of this user

Scenario: Af01. Select with ticket that doesn't exist
	Given I pass a ticket that doesn't exist
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Af02. Select with ticket that is not active anymore
	Given I pass a ticket that is already disabled
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Af03. Select with info all right
	Given I pass a ticket that exist
	When I try to get the session
	Then I will receive no core error
		And I will receive the session

Scenario: Af04. Select with empty ticket
	Given I pass an empty ticket
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Af05. Select with null ticket
	Given I pass a null ticket
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session
