Feature: Aw. Save access

Background:
	Given test user login

Scenario: Aw01. Save access for current user
	When I try to save the access
	Then I will receive no core error
		And the access will be after test start time

Scenario: Aw02. Save access for no user
	Given I have no logged user (logoff)
	When I try to save the access
	Then I will receive no core error
		And the access will not be after test start time
