Feature: E-mail sending

Scenario: 01. Send an e-mail without subject
	Given I have this e-mail to send
		| Subject | Body            | To                |
		|         | without subject | [email-test-here] |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidSubject

Scenario: 02. Send an e-mail without body
	Given I have this e-mail to send
		| Subject      | Body | To                |
		| without body |      | [email-test-here] |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidBody

Scenario: 03. Send an e-mail without addressee
	Given I have this e-mail to send
		| Subject           | Body              | To |
		| without addressee | without addressee |    |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidAddressee

Scenario: 98. Send an e-mail successfuly
	Given I have this e-mail to send
		| Subject | Body  | To                |
		| right   | right | [email-test-here] |
	When I try to send the e-mail
	Then I will receive no e-mail error

Scenario: 99. Send an e-mail to default
	Given I have this e-mail to send to default
		| Subject | Body    |
		| default | default |
	When I try to send the e-mail
	Then I will receive no e-mail error
