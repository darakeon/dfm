Feature: Aj. Disable ticket

Background:
	Given I have this user created
			| Email                          | Password | Retype Password | Active |
			| disableticket@dontflymoney.com | password | password        | true   |
		And I have a ticket of this user

Scenario: Aj01. Disable with info all right
	Given I pass a ticket that exist
	When I try to disable the ticket
	Then I will receive no core error
		And the ticket will not be valid anymore

Scenario: Aj02. Disable with inexistent ticket
	Given I pass a ticket that doesn't exist
	When I try to disable the ticket
	Then I will receive no core error

Scenario: Aj03. Disable with null ticket
	Given I pass a null ticket
	When I try to disable the ticket
	Then I will receive no core error

Scenario: Aj04. Disable with wrong user
	Given I pass a ticket that exist
		But there is a bad person logged in
	When I try to disable the ticket
	Then I will receive this core error: Uninvited

Scenario: Aj05. Disable with empty ticket
	Given I pass an empty ticket
	When I try to disable the ticket
	Then I will receive no core error
