Feature: Gf. Reset password

Background:
	Given I have this user created
			| Email                           | Password  | Signed | Active |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have a ticket of this user
		And I have a token for its password reset

Scenario: Gf01. Password reset with invalid token
	Given I pass an invalid token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Gf02. Password reset with token of user verification
	Given I have a token for its activation
		And I pass a valid UserVerification token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Gf03. Password reset with no password
	Given I pass a valid PasswordReset token
		And I pass no password
	When I try to reset the password
	Then I will receive this core error: UserPasswordRequired
		And the password will not be changed

Scenario: Gf04. Password reset with info all right
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive no core error
		And the password will be changed
		And the token will not be valid anymore
		And there will be no active logins

Scenario: Gf05. Password reset with token of unsubscribe move mail
	Given I have a token for its activation
		And I pass a valid UnsubscribeMoveMail token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Gf06. Password reset with token already used
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
	When I try to reset the password
	Then I will receive no core error
	# Same token
	When I try to reset the password
	Then I will receive this core error: InvalidToken

Scenario: Gf07. Not reset if user is marked for deletion
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the user is marked for deletion
	When I try to reset the password
	Then I will receive this core error: UserDeleted

Scenario: Gf08. Not reset if user requested wipe
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the user asked data wipe
	When I try to reset the password
	Then I will receive this core error: UserAskedWipe

Scenario: Gf09. Password reset with expired token
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password     | Retype Password |
			| new_password | new_password    |
		But the token expires
	When I try to reset the password
	Then I will receive this core error: InvalidToken
		And the password will not be changed

Scenario: Gf10. Save user with too short password
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password | Retype Password |
			| pass     | pass            |
	When I try to reset the password
	Then I will receive this core error: UserPasswordTooShort
		And the password will not be changed

Scenario: Gf11. Save user with too long password
	Given I pass a valid PasswordReset token
		And I pass this data to change password
			| Password                                                                                                                                                                                                                                                                                                                                                                    | Retype Password                                                                                                                                                                                                                                                                                                                                                             |
			| by_sending_a_very_long_password_like_1000000_characters_it_is_possible_to_cause_a_denial_a_service_attack_on_the_server-this_may_lead_to_the_website_becoming_unavailable_or_unresponsive-usually_this_problem_is_caused_by_a_vulnerable_password_hashing_implementation-when_a_long_password_is_sent_the_password_hashing_process_will_result_in_cpu_and_memory_exhaustion | by_sending_a_very_long_password_like_1000000_characters_it_is_possible_to_cause_a_denial_a_service_attack_on_the_server-this_may_lead_to_the_website_becoming_unavailable_or_unresponsive-usually_this_problem_is_caused_by_a_vulnerable_password_hashing_implementation-when_a_long_password_is_sent_the_password_hashing_process_will_result_in_cpu_and_memory_exhaustion |
	When I try to reset the password
	Then I will receive this core error: UserPasswordTooLong
		And the password will not be changed
