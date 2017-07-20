Feature: Reset password of user

Background:
	Given I have an user
	And I have a token for its password reset

Scenario: A601. Password reset with invalid token (E)
	Given I pass an invalid token
	When I try to reset the password
	Then I will receive this error
		| Error        |
		| InvalidToken |
	And the password will not be changed

Scenario: A602. Password reset with token of user verification (E)
	Given I pass a token of user verification
	When I try to reset the password
	Then I will receive this error
		| Error        |
		| InvalidToken |
	And the password will not be changed

Scenario: A603. Password reset with no password (E)
	Given I pass no password
	When I try to reset the password
	Then I will receive this error
		| Error       		|
		| UserPasswordRequired |
	And the password will not be changed

Scenario: A699. Password reset with info all right (S)
	Given I pass the valid token
	When I try to reset the password
	Then I will receive no error
	And the password will be changed