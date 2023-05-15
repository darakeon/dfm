Feature: Dc. Dismiss tip

Background:
	Given test user login

Scenario: Dc01. Dismiss no tip
	Given asked for tip 2 times
	When dismiss tip
	Then I will receive no core error
		And the next tip will shown after 3 requests

Scenario: Dc02. Dismiss tip
	Given asked for tip 5 times
	When dismiss tip
	Then I will receive no core error
		And the next tip will shown after 5 requests

Scenario: Dc03. Not dismiss tip if user is marked for deletion
	Given the user is marked for deletion
	When dismiss tip
	Then I will receive this core error: UserDeleted

Scenario: Dc04. Not dismiss tip if user requested wipe
	Given the user asked data wipe
	When dismiss tip
	Then I will receive this core error: UserAskedWipe

Scenario: Dc05. Not dismiss tip if user logoff
	Given I logoff the user
	When dismiss tip
	Then I will receive this core error: Uninvited

Scenario: Dc06. Not dismiss tip if not signed last contract
	Given there is a new contract
	When dismiss tip
	Then I will receive this core error: NotSignedLastContract
