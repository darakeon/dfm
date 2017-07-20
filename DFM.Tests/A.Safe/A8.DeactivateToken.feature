Feature: Deactivate the user

Scenario: A801. Deactivate with invalid token (E)
	Given I pass an invalid token
	When I try do deactivate the token
	Then I will receive this error
		| Error        |
		| InvalidToken |

Scenario: A899. Deactivate with info all right (S)
	Given I pass a valid token
	When I try do deactivate the token
	Then I will receive no error
	And the token will not be valid anymore