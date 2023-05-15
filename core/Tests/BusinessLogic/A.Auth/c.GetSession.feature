Feature: Ac. Get session

Background:
	Given I have this user created
			| Email                           | Password | Active | Signed |
			| {scenarioCode}@dontflymoney.com | password | true   | true   |
		And I have this user data
			| Email                           | Password |
			| {scenarioCode}@dontflymoney.com | password |
		And I have a ticket of this user

Scenario: Ac01. Select with ticket that doesn't exist
	Given I pass a ticket that doesn't exist
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Ac02. Select with ticket that is not active anymore
	Given I pass a ticket that is already disabled
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Ac03. Select with info all right
	Given I pass a ticket that exist
	When I try to get the session
	Then I will receive no core error
		And I will receive the session

Scenario: Ac04. Select with empty ticket
	Given I pass an empty ticket
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Ac05. Select with null ticket
	Given I pass a null ticket
	When I try to get the session
	Then I will receive this core error: Uninvited
		And I will receive no session

Scenario: Ac06. Has TFA
	Given I have this two-factor data
			| Secret | Code        | Password |
			| 123    | {generated} | password |
		And I set two-factor
		And I pass a ticket that exist
	When I try to get the session
	Then I will receive no core error
		And I will receive the session
		And the TFA will be enabled

Scenario: Ac07. Has no TFA
	Given I pass a ticket that exist
	When I try to get the session
	Then I will receive no core error
		And I will receive the session
		And the TFA will not be enabled

Scenario: Ac08. Not get session if user is marked for deletion
	Given I pass a ticket that exist
		But the user is marked for deletion
	When I try to get the session
	Then I will receive this core error: UserDeleted

Scenario: Ac09. Not get session if user requested wipe
	Given I pass a ticket that exist
		But the user asked data wipe
	When I try to get the session
	Then I will receive this core error: UserAskedWipe
