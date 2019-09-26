Feature: Av. Verify Ticket Type

Background:
	Given I have this user created and activated
			| Email               | Password |
			| Av@dontflymoney.com | password |
		And I login this user
			| Email               | Password |
			| Av@dontflymoney.com | password |

Scenario: Av01. Verify ticket as mobile
	Given I have a ticket key
	When I try to verify the ticket type to be mobile
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Av02. Verify ticket as browser
	Given I have a ticket key
	When I try to verify the ticket type to be browser
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Av03. Verify ticket as local
	Given I have a ticket key
	When I try to verify the ticket type to be local
	Then I will receive no core error
		And the ticket will be verified
