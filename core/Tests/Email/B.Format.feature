Feature: B. Format

Background:
	Given an user

Scenario: B01. Format move notification
	When a move notification is formatted
		And the e-mail is sent
	Then there will be a move-notification e-mail sent

Scenario: B02. Format security action user verification
	When a security action is formatted to UserVerification
		And the e-mail is sent
	Then there will be a security-action-user-verification e-mail sent

Scenario: B03. Format security action password reset
	When a security action is formatted to PasswordReset
		And the e-mail is sent
	Then there will be a security-action-password-reset e-mail sent

Scenario: B04. Format removal by no interaction
	When a user removal is formatted because of NoInteraction
		And the e-mail is sent
	Then there will be a removal-reason-no-interaction e-mail sent

Scenario: B05. Format removal by not signed contract
	When a user removal is formatted because of NotSignedContract
		And the e-mail is sent
	Then there will be a removal-reason-not-signed-contract e-mail sent

Scenario: B06. Format purge by no interaction notice
	When a purge notice is formatted because of NoInteraction
		And the e-mail is sent
	Then there will be a purge-notice-no-interaction e-mail sent

Scenario: B07. Format purge by not signed contract notice
	When a purge notice is formatted because of NotSignedContract
		And the e-mail is sent
	Then there will be a purge-notice-not-signed-contract e-mail sent
