Feature: j. Disable Ticket

Background:
	Given I have this user to create and activate
		| Email                          | Password |
		| disableticket@dontflymoney.com | password |
	And I have a ticket of this user

Scenario: 01. Select with ticket that doesn't exist (E)
	Given I pass a ticket that doesn't exist
	When I try to disable the ticket
	Then I will receive this core error: InvalidTicket

Scenario: 02. Select with ticket that is not active anymore (E)
	Given I pass a ticket that is already disabled
	When I try to disable the ticket
	Then I will receive this core error: InvalidTicket

Scenario: 99. Select with info all right (S)
	Given I pass a ticket that exist
	When I try to disable the ticket
	Then I will receive no core error
	And the ticket will not be valid anymore