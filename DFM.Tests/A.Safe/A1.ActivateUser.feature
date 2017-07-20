Feature: Activate the user

Background:
	Given I have a user
	And I have a valid token for this user actvation

Scenario: A101. Activate user with invalid token (E)
	Given I pass an invalid token
	When I try to activate the user
	Then I will receive this error
         | Error        |
         | InvalidToken |
	And the user will not be activated

Scenario: A102. Activate user with token of reset password (E)
	Given I pass a token of password reset 
	When I try to activate the user
	Then I will receive this error
         | Error        |
         | InvalidToken |
	And the user will not be activated

Scenario: A199. Activate user with info all right (S)
	Given I pass the valid token
	When I try to activate the user
	Then I will receive no error
	And the user will be activated