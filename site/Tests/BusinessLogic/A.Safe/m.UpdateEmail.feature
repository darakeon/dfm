Feature: Am. Update E-mail

Scenario: Am01. E-mail change with wrong current password
	Given I have this user created and activated
			| Email                 | Password |
			| Am01@dontflymoney.com | password |
		And I have a ticket of this user
		And I pass this new e-mail and password
			| New E-mail             | Current Password |
			| Am01_@dontflymoney.com | password_wrong   |
	When I try to change the e-mail
	Then I will receive this core error: WrongPassword
		And the e-mail will not be changed
		And the user will be activated

Scenario: Am02. E-mail change with empty new e-mail
	Given I have this user created and activated
			| Email                 | Password |
			| Am02@dontflymoney.com | password |
		And I have a ticket of this user
		And I pass this new e-mail and password
			| New E-mail | Current Password |
			|            | password         |
	When I try to change the e-mail
	Then I will receive this core error: UserEmailInvalid
		And the e-mail will not be changed
		And the user will be activated

Scenario: Am03. E-mail change with info all right
	Given I have this user created and activated
			| Email                 | Password |
			| Am99@dontflymoney.com | password |
		And I have a ticket of this user
		And I pass this new e-mail and password
			| New E-mail             | Current Password |
			| Am99_@dontflymoney.com | password         |
	When I try to change the e-mail
	Then I will receive no core error
		And the e-mail will be changed
		And the user will not be activated
