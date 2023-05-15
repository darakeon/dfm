Feature: Bd. Save access

Background:
	Given test user login
		And I set test start date here

Scenario: Bd01. Save access for current user
	When I try to save the access
	Then I will receive no core error
		And the ticket access will be after test start time
		And the user access will not be null

Scenario: Bd02. Save access for no user
	Given I have no logged user (logoff)
	When I try to save the access
	Then I will receive no core error
		And the ticket access will not be after test start time
		And the user access will be null

Scenario: Bd03. Save access clear warning counting
	Given the user have being warned twice
	When I try to save the access
	Then I will receive no core error
		And and the user warning count will be 0

Scenario: Bd04. Not save access if user is marked for deletion
	Given the user is marked for deletion
	When I try to save the access
	Then I will receive this core error: UserDeleted

Scenario: Bd05. Not save access if user requested wipe
	Given the user asked data wipe
	When I try to save the access
	Then I will receive this core error: UserAskedWipe
