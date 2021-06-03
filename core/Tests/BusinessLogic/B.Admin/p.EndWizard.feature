﻿Feature: Bp. End wizard

Background:
	Given test user login

Scenario: Bp01. End wizard
	Given I enable wizard
	When I end wizard
	Then I will receive no core error

Scenario: Bp02. End Wizard logged out
	Given I have no logged user (logoff)
	When I end wizard
	Then I will receive this core error: Uninvited

Scenario: Bp03. Not end wizard if user is marked for deletion
	Given the user is marked for deletion
	When I end wizard
	Then I will receive this core error: UserDeleted
