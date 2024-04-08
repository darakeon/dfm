Feature: Cf. Reopen account

Background:
	Given test user login
		And these settings
			| UseCategories |
			| false         |

Scenario: Cf01. Reopen an Account that doesn't exist
	Given I pass a url of account that doesn't exist
	When I try to reopen the account
	Then I will receive this core error: InvalidAccount

Scenario: Cf02. Reopen an Account that is not closed
	Given I give a url of the account Bo02 with moves
	When I try to reopen the account
	Then I will receive this core error: OpenedAccount

Scenario: Cf03. Reopen an Account with info all right
	Given I give a url of the account Bo03 with moves
		And I already have closed the account
	When I try to reopen the account
	Then I will receive no core error
		And the account will be open
		And the account will not have an end date

Scenario: Cf04. Reopen an Account logged out
	Given I give a url of the account Bo04 with moves
		And I already have closed the account
		And I have no logged user (logoff)
	When I try to reopen the account
	Then I will receive this core error: Uninvited

Scenario: Cf05. Not reopen an Account if user is marked for deletion
	Given I give a url of the account Bo05 with moves
		And I already have closed the account
		But the user is marked for deletion
	When I try to reopen the account
	Then I will receive this core error: UserDeleted

Scenario: Cf06. Not reopen an Account if user requested wipe
	Given I give a url of the account Bo06 with moves
		And I already have closed the account
		But the user asked data wipe
	When I try to reopen the account
	Then I will receive this core error: UserAskedWipe

Scenario: Cf07. Not reopen an Account if not signed last contract
	Given I give a url of the account Bo07 with moves
		And I already have closed the account
		But there is a new contract
	When I try to reopen the account
	Then I will receive this core error: NotSignedLastContract
