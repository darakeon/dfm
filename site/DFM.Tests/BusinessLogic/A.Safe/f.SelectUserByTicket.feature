Feature: Af. Get an user by its ticket

Background:
	Given I have this user created and activated
		| Email                               | Password | Retype Password |
		| selectuserbyticket@dontflymoney.com | password | password        |
	And I have a ticket of this user

Scenario: Af01. Select with ticket that doesn't exist (E)
	Given I pass a ticket that doesn't exist
	When I try to get the user
	Then I will receive this core error: Uninvited
	And I will receive no user

Scenario: Af02. Select with ticket that is not active anymore (E)
	Given I pass a ticket that is already disabled
	When I try to get the user
	Then I will receive this core error: Uninvited
	And I will receive no user

Scenario: Af99. Select with info all right (S)
	Given I pass a ticket that exist
	When I try to get the user
	Then I will receive no core error
	And I will receive the user