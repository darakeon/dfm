Feature: Aw. Save access

Background:
	Given test user login
		And I set test start date here

Scenario: Aw01. Save access for current user
	When I try to save the access
	Then I will receive no core error
		And the ticket access will be after test start time
		And the user access will not be null

Scenario: Aw02. Save access for no user
	Given I have no logged user (logoff)
	When I try to save the access
	Then I will receive no core error
		And the ticket access will not be after test start time
		And the user access will be null

Scenario: Aw03. Save access clear warning counting
	Given the user have being warned twice
	When I try to save the access
	Then I will receive no core error
		And and the user warning count will be 0
