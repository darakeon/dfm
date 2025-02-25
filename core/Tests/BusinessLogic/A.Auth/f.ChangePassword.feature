Feature: Af. Change password

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I have this user data
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have a ticket of this user

Scenario: Af01. Password change with wrong current password
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| password_wrong   | new_password | new_password    |
	When I try to change the password
	Then I will receive this core error: WrongPassword
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af02. Password change with empty new password
	Given I pass this data to change password
			| Current Password | Password | Retype Password |
			| pass_word        |          | new_password    |
	When I try to change the password
	Then I will receive this core error: UserPasswordRequired
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af03. Password change with different retype password
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | password_wrong  |
	When I try to change the password
	Then I will receive this core error: RetypeWrong
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af04. Password change with info all right
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | new_password    |
	When I try to change the password
	Then I will receive no core error
		And the password will be changed
		And only the last login will be active

Scenario: Af05. Not change if user is marked for deletion
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | new_password    |
		But the user is marked for deletion
	When I try to change the password
	Then I will receive this core error: UserDeleted

Scenario: Af06. Not change if user requested wipe
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | new_password    |
		But the user asked data wipe
	When I try to change the password
	Then I will receive this core error: UserAskedWipe

Scenario: Af07. Change Password without signing contract
	Given I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | new_password    |
		But there is a new contract
	When I try to change the password
	Then I will receive this core error: NotSignedLastContract

Scenario: Af08. Save user with too short password
	Given I pass this data to change password
			| Current Password | Password | Retype Password |
			| pass_word        | pass     | pass            |
	When I try to change the password
	Then I will receive this core error: UserPasswordTooShort

Scenario: Af09. Save user with too long password
	Given I pass this data to change password
			| Current Password | Password                                                                                                                                                                                                                                                                                                                                                                    | Retype Password                                                                                                                                                                                                                                                                                                                                                             |
			| pass_word        | by_sending_a_very_long_password_like_1000000_characters_it_is_possible_to_cause_a_denial_a_service_attack_on_the_server-this_may_lead_to_the_website_becoming_unavailable_or_unresponsive-usually_this_problem_is_caused_by_a_vulnerable_password_hashing_implementation-when_a_long_password_is_sent_the_password_hashing_process_will_result_in_cpu_and_memory_exhaustion | by_sending_a_very_long_password_like_1000000_characters_it_is_possible_to_cause_a_denial_a_service_attack_on_the_server-this_may_lead_to_the_website_becoming_unavailable_or_unresponsive-usually_this_problem_is_caused_by_a_vulnerable_password_hashing_implementation-when_a_long_password_is_sent_the_password_hashing_process_will_result_in_cpu_and_memory_exhaustion |
	When I try to change the password
	Then I will receive this core error: UserPasswordTooLong

Scenario: Af10. TFA activated but without code
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this data to change password
			| Current Password | Password     | Retype Password |
			| pass_word        | new_password | new_password    |
	When I try to change the password
	Then I will receive this core error: TFAWrongCode
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af11. TFA activated but with empty code
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this data to change password
			| Current Password | Password     | Retype Password | TFA Code |
			| pass_word        | new_password | new_password    |          |
	When I try to change the password
	Then I will receive this core error: TFAWrongCode
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af12. TFA activated but with wrong code
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this data to change password
			| Current Password | Password     | Retype Password | TFA Code |
			| pass_word        | new_password | new_password    | 123456   |
	When I try to change the password
	Then I will receive this core error: TFAWrongCode
		And the password will not be changed
		And the ticket will still be valid

Scenario: Af13. TFA activated with right code
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I validate the ticket two factor
		And I pass this data to change password
			| Current Password | Password     | Retype Password | TFA Code    |
			| pass_word        | new_password | new_password    | {generated} |
	When I try to change the password
	Then I will receive no core error
		And the password will be changed
		And only the last login will be active
