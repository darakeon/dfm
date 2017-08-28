Feature: j. Disable Ticket

Background:
	Given I have this user created and activated
		| Email                          | Password | Retype Password |
		| disableticket@dontflymoney.com | password | password        |
	And I have a ticket of this user

Scenario: 99. Select with info all right (S)
	Given I pass a ticket that exist
	When I try to disable the ticket
	Then I will receive no core error
	And the ticket will not be valid anymore