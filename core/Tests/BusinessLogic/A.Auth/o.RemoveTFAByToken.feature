Feature: Ao. Remove TFA by Token

Background:
	Given I have this user created
			| Email                           | Password  | Active | Signed |
			| {scenarioCode}@dontflymoney.com | pass_word | true   | true   |
		And I login this user
			| Email                           | Password  |
			| {scenarioCode}@dontflymoney.com | pass_word |
		And I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
		And I have a token for its tfa removal

Scenario: Ao01. Remove tfa with invalid token
	Given I pass an invalid token
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao02. Remove tfa with token of reset password
	Given I have a token for its password reset
		And I pass a valid PasswordReset token
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao03. Remove tfa with token of user verification
	Given I have a token for its activation
		And I pass a valid UserVerification token
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao04. Remove tfa with token of unsubscribe move mail
	Given I have a token for its password reset
		And I pass a valid UnsubscribeMoveMail token
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao05. Remove tfa with info all right
	Given I pass a valid RemoveTFA token
	When remove tfa by token
	Then I will receive no core error
		And the two-factor will be empty
		And the token will not be valid anymore

Scenario: Ao06. Remove tfa with token already used
	Given I pass a valid RemoveTFA token
	When remove tfa by token
	Then I will receive no core error
		And the two-factor will be empty
	Given I have this two-factor data
			| Secret | Code        | Password  |
			| 123    | {generated} | pass_word |
		And I set two-factor
	# Same token
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao07. Remove tfa user is marked for deletion
	Given I pass a valid RemoveTFA token
		But the user is marked for deletion
	When remove tfa by token
	Then I will receive this core error: UserDeleted

Scenario: Ao08. Remove tfa user requested wipe
	Given I pass a valid RemoveTFA token
		But the user asked data wipe
	When remove tfa by token
	Then I will receive this core error: UserAskedWipe

Scenario: Ao09. Remove tfa user not signed last contract
	Given I pass a valid RemoveTFA token
		But there is a new contract
	When remove tfa by token
	Then I will receive this core error: NotSignedLastContract
		And the two-factor will be [123]

Scenario: Ao10. Remove tfa with expired token
	Given I pass a valid RemoveTFA token
		But the token expires
	When remove tfa by token
	Then I will receive this core error: InvalidToken
		And the two-factor will be [123]

Scenario: Ao11. Remove tfa other user logged in
	Given I pass a valid RemoveTFA token
		But there is a bad person logged in
	When remove tfa by token
	Then I will receive this core error: Uninvited
		And the two-factor will be [123]

Scenario: Ao12. Remove tfa no user logged in
	Given I pass a valid RemoveTFA token
		But I logoff the user
	When remove tfa by token
	Then I will receive this core error: Uninvited
		And the two-factor will be [123]

Scenario: Ao13. Remove tfa with not tfa
	Given I pass a valid RemoveTFA token
		But I remove two-factor
	When remove tfa by token
	Then I will receive this core error: TFANotConfigured
		And the two-factor will be empty

Scenario: Ao14. Remove tfa if set to use as password
	Given I set to use TFA as password
		And I pass a valid RemoveTFA token
	When remove tfa by token
	Then I will receive no core error
		And the two-factor will be empty
		And the token will not be valid anymore
		And the TFA can not be used as password
