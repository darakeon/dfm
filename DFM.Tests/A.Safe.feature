Feature: Create and security of User

@SaveUserAndSendVerify
Scenario: 001. Save user without email (E)
	Given I have this user to create
	| Email | Password |
	|       | testDFM  |
	When I try to save the user
	Then I will receive this error
	| Error            |
	| UserInvalidEmail |
	And the user will not be saved

Scenario: 002. Save user without password (E)
	Given I have this user to create
	| Email                 | Password |
	| test@dontflymoney.com | testDFM  |
	When I try to save the user
	Then I will receive this error
	| Error            |
	| UserInvalidEmail |
	And the user will not be saved

Scenario: 003. Save user with invalid email (E)
Scenario: 003. Save user with repeated email (E)
Scenario: 099. Save user with info all right (S)

@ActivateUser
Scenario: 101. Activate user with invalid token (E)
Scenario: 102. Activate user with token of reset password (E)
Scenario: 199. Activate user with info all right (S)

@ValidateAndGetUser
Scenario: 201. Validate without email (E)
Scenario: 202. Validate without password (E)
Scenario: 203. Validate with wrong email (E)
Scenario: 204. Validate with wrong password (E)
Scenario: 299. Validate with info all right (S)

@SelectUserByEmail
Scenario: 301. Select with email that doesn't exist (E)
Scenario: 399. Select with info all right (S)

@SendUserVerify
Scenario: 401. Send with email that doesn't exist (E)
Scenario: 499. Send with info all right (S)

@SendPasswordReset
Scenario: 501. Send with email that doesn't exist (E)
Scenario: 599. Send with info all right (S)

@PasswordReset
Scenario: 601. Password reset with invalid token (E)
Scenario: 602. Password reset with token of validate user (E)
Scenario: 603. Password reset with no password (E)
Scenario: 699. Password reset with info all right (S)

@TestSecurityToken
Scenario: 701. Test with no token (E)
Scenario: 702. Test with token of other action (E)
Scenario: 799. Test with info all right (S)

@Deactivate
Scenario: 801. Deactivate with invalid token (E)
Scenario: 899. Deactivate with info all right (S)