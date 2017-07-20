Feature: Test security token received by e-mail

Background:
	Given I have an user
	And I have a token for its password reset
	And I have a token for its actvation

Scenario: A701. Test with invalid token (E)
	Given I pass an invalid token
	When I test the token
	Then I will receive this error
		| Error        |
		| InvalidToken |

Scenario: A702. Test with token of UV with action PS (E)
	Given I pass a token of user verification with action password reset
	When I test the token
	Then I will receive this error
		| Error        |
		| InvalidToken |

Scenario: A703. Test with token of PS with action UV (E)
	Given I pass a token of password reset with action user verification
	When I test the token
	Then I will receive this error
		| Error        |
		| InvalidToken |

Scenario: A798. Test with token of UV with action UV (S)
	Given I pass a token of user verification with right action
	When I test the token
	Then I will receive no error

Scenario: A799. Test with token of PS with action PS (S)
	Given I pass a token of password reset with right action
	When I test the token
	Then I will receive no error