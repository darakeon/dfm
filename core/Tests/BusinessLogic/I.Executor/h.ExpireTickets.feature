Feature: Ih. Expire Tickets

Background:
	Given test user login

Scenario: Ih01. Unlogged user
	Given the ticket was last accessed 31 days ago
		But I have no logged user (logoff)
	When expire tickets not accessed anymore
	Then I will receive this core error: Uninvited
		And the ticket will still be valid

Scenario: Ih02. No robot user
	Given the ticket was last accessed 31 days ago
		But test user login
	When expire tickets not accessed anymore
	Then I will receive this core error: Uninvited
		And the ticket will still be valid

Scenario: Ih03. User marked for deletion
	Given the ticket was last accessed 31 days ago
		But the user is marked for deletion
	When robot user login
		And expire tickets not accessed anymore
	Then I will receive no core error
		And the ticket will not be valid anymore

Scenario: Ih04. User requested wipe
	Given the ticket was last accessed 31 days ago
		But the user asked data wipe
	When robot user login
		And expire tickets not accessed anymore
	Then I will receive no core error
		And the ticket will not be valid anymore

Scenario: Ih05. Without sign last contract
	Given the ticket was last accessed 31 days ago
		But there is a new contract
	When robot user login
		And expire tickets not accessed anymore
	Then I will receive no core error
		And the ticket will not be valid anymore

Scenario: Ih06. Expired ticket
	Given the ticket was last accessed 31 days ago
	When robot user login
		And expire tickets not accessed anymore
	Then I will receive no core error
		And the ticket will not be valid anymore

Scenario: Ih07. Not yet expired
	Given the ticket was last accessed 29 days ago
	When robot user login
		And expire tickets not accessed anymore
	Then I will receive no core error
		And the ticket will still be valid
