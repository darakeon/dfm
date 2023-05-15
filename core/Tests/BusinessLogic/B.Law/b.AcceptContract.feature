Feature: Bb. Accept contract

Background:
	Given test user login
	And I have a contract

Scenario: Bb01. Accept contract not accepted before
	When I try to accept the contract
	Then I will receive no core error
		And the contract status will be accepted

Scenario: Bb02. Accept contract accepted before
	Given I have accepted the contract
	When I try to accept the contract
	Then I will receive no core error
		And the contract status will be accepted

Scenario: Bb03. Accept contract clear warning counting
	Given the user have being warned twice
	When I try to accept the contract
	Then I will receive no core error
		And and the user warning count will be 0

Scenario: Bb04. Not accept contract if user is marked for deletion
	Given the user is marked for deletion
	When I try to accept the contract
	Then I will receive this core error: UserDeleted

Scenario: Bb05. Not accept contract if user requested wipe
	Given the user asked data wipe
	When I try to accept the contract
	Then I will receive this core error: UserAskedWipe
