Feature: Dd. Disable tip

Background:
	Given test user login

Scenario: Dd01. Disable tip
	When disable tip TestTip1
	Then I will receive no core error
		And the TestTip1 will be disabled

Scenario: Dd02. Not disable tip if user is marked for deletion
	Given the user is marked for deletion
	When disable tip TestTip1
	Then I will receive this core error: UserDeleted

Scenario: Dd03. Not disable tip if user requested wipe
	Given the user asked data wipe
	When disable tip TestTip1
	Then I will receive this core error: UserAskedWipe

Scenario: Dd04. Not disable tip if user logoff
	Given I logoff the user
	When disable tip TestTip1
	Then I will receive this core error: Uninvited

Scenario: Dd05. Not disable tip if not signed last contract
	Given there is a new contract
	When disable tip TestTip1
	Then I will receive this core error: NotSignedLastContract
