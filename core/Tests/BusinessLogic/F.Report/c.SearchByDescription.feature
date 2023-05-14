Feature: Ec. Search by description

Background:
	Given test user login
		And I enable Categories use
		And I have an account
		And I have moves of
			| Description            | Date       | Detail            |
			| Chocolate              | 2020-09-21 |                   |
			| Market                 | 2020-09-22 | Chocolate         |
			| Cheese                 | 2020-09-23 |                   |
			| Bakery                 | 2020-09-24 | Cheese            |
			| Chocolate again        | 2020-09-25 | So much chocolate |
			| Much tons of Chocolate | 2020-09-26 |                   |

Scenario: Ec01. Search with result
	When I try to search by description Choco
	Then I will receive no core error
		And I will receive these moves
			| Description            | Date       | Detail            |
			| Chocolate              | 2020-09-21 |                   |
			| Market                 | 2020-09-22 | Chocolate         |
			| Chocolate again        | 2020-09-25 | So much chocolate |
			| Much tons of Chocolate | 2020-09-26 |                   |

Scenario: Ec02. Search without result
	When I try to search by description something-that-does-not-exists
	Then I will receive no core error
		And I will receive no moves

Scenario: Ec03. Search with different case result
	When I try to search by description choco
	Then I will receive no core error
		And I will receive these moves
			| Description            | Date       | Detail            |
			| Chocolate              | 2020-09-21 |                   |
			| Market                 | 2020-09-22 | Chocolate         |
			| Chocolate again        | 2020-09-25 | So much chocolate |
			| Much tons of Chocolate | 2020-09-26 |                   |

Scenario: Ec04. Search with spaces
	When I try to search by description choco much
	Then I will receive no core error
		And I will receive these moves
			| Description            | Date       | Detail            |
			| Chocolate again        | 2020-09-25 | So much chocolate |
			| Much tons of Chocolate | 2020-09-26 |                   |

Scenario: Ec05. Search from different user
	Given there is another person logged in
	When I try to search by description Choco
	Then I will receive no core error
		And I will receive no moves

Scenario: Ec06. Search by null
	When I try to search by description {null}
	Then I will receive no core error
		And I will receive no moves

Scenario: Ec07. Search by empty
	When I try to search by description {empty}
	Then I will receive no core error
		And I will receive no moves

Scenario: Ec08. Search logged off
	Given I have no logged user (logoff)
	When I try to search by description Choco
	Then I will receive this core error: Uninvited

Scenario: Ec09. Not search if user is marked for deletion
	Given the user is marked for deletion
	When I try to search by description Choco
	Then I will receive this core error: UserDeleted

Scenario: Ec10. Not search if user requested wipe
	Given the user asked data wipe
	When I try to search by description Choco
	Then I will receive this core error: UserAskedWipe

Scenario: Ec11. Not search if not signed last contract
	Given there is a new contract
	When I try to search by description Choco
	Then I will receive this core error: NotSignedLastContract
