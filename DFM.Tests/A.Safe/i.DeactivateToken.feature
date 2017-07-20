Feature: i. Deactivate the user

Background:
	Given I have an user

Scenario: 01. Deactivate with invalid token (E)
	Given I pass an invalid token
	When I try do deactivate the token
	Then I will receive this error
		| Error        |
		| InvalidToken |

Scenario: 99. Deactivate with info all right (S)
	Given I pass a valid token
	When I try do deactivate the token
	Then I will receive no error
	And the token will not be valid anymore