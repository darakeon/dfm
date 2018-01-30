Feature: E-mail sending

Scenario: MS01. Send an e-mail without subject
	Given I have this e-mail to send
			| Subject | Body            | To                   |
			|         | without subject | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidSubject

Scenario: MS02. Send an e-mail without body
	Given I have this e-mail to send
			| Subject      | Body | To                   |
			| without body |      | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidBody

Scenario: MS03. Send an e-mail without address
	Given I have this e-mail to send
			| Subject           | Body              | To |
			| without addressee | without addressee |    |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidAddress

Scenario: MS04. Send an e-mail successfuly
	Given I have this e-mail to send
			| Subject | Body  | To                   |
			| right   | right | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive no e-mail error

Scenario: MS05. Send an e-mail to default
	Given I have this e-mail to send to default
			| Subject | Body    |
			| default | default |
	When I try to send the e-mail
	Then I will receive no e-mail error
