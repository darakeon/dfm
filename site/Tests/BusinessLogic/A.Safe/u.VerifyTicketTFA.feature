Feature: Au. Verify Ticket TFA

Background:
	Given I have this user created and activated
			| Email               | Password |
			| Au@dontflymoney.com | password |
		And I login this user
			| Email               | Password |
			| Au@dontflymoney.com | password |
		And I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor

Scenario: Au01. Check not verified ticket
	Given I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will not be verified

Scenario: Au02. Check verified ticket
	Given I have a ticket key
		And I have this two-factor data
			| Secret | Code        |
			| 123    | {generated} |
		And I validate the ticket two factor
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified

Scenario: Au03. Check ticket without two factor
	Given I remove two-factor
		And I have a ticket key
	When I try to verify the ticket
	Then I will receive no core error
		And the ticket will be verified
