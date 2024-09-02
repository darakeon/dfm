Feature: A. E-mail send

Scenario: A01. Send an e-mail without subject
	Given I have this e-mail to send
			| Subject | Body            | To                   |
			|         | without subject | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidSubject

Scenario: A02. Send an e-mail without body
	Given I have this e-mail to send
			| Subject      | Body | To                   |
			| without body |      | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidBody

Scenario: A03. Send an e-mail without address
	Given I have this e-mail to send
			| Subject           | Body              | To |
			| without addressee | without addressee |    |
	When I try to send the e-mail
	Then I will receive this e-mail error: InvalidAddress

Scenario: A04. Send an e-mail successfuly
	Given I have this e-mail to send
			| Subject | Body  | To                   |
			| right   | right | dfm@dontflymoney.com |
	When I try to send the e-mail
	Then I will receive no e-mail error

Scenario: A05. Send an e-mail to default
	Given I have this e-mail to send to default
			| Subject | Body    |
			| default | default |
	When I try to send the e-mail
	Then I will receive no e-mail error

Scenario: A06. Send an e-mail with unsubscribe link
	Given I have this e-mail to send
			| Subject | Body  | To                     | Unsubscribe Link                    |
			| unsub   | unsub | unsub@dontflymoney.com | https://dontlfymoney.com/fake-unsub |
	When I try to send the e-mail
	Then I will receive no e-mail error
		And there will be a header in the email sent to unsub@dontflymoney.com with the link https://dontlfymoney.com/fake-unsub

Scenario: A07. With attachments
	Given I have this e-mail to send
			| Subject | Body   | To                      | Attachment |
			| attach  | attach | attach@dontflymoney.com | attach.txt |
	When I try to send the e-mail
	Then I will receive no e-mail error
		And there will be an attachment in the email sent to attach@dontflymoney.com with the content of attach.txt
