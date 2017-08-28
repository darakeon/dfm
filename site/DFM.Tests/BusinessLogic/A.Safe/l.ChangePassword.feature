Feature: l. Change password

Scenario: 01. Password change with wrong current password (E)
	Given I have this user created and activated
		| Email                 | Password |
		| Al01@dontflymoney.com | password |
	And I have a ticket of this user
	And I pass this password
		| Current Password | Password     | Retype Password |
		| password_wrong   | new_password | new_password    |
	When I try to change the password
	Then I will receive this core error: WrongPassword
	And the password will not be changed
	And the ticket will still be valid

Scenario: 02. Password change with empty new password (E)
	Given I have this user created and activated
		| Email                 | Password |
		| Al02@dontflymoney.com | password |
	And I have a ticket of this user
	And I pass this password
		| Current Password | Password | Retype Password |
		| password         |          | new_password    |
	When I try to change the password
	Then I will receive this core error: UserPasswordRequired
	And the password will not be changed
	And the ticket will still be valid

Scenario: 03. Password change with different retype password (E)
	Given I have this user created and activated
		| Email                 | Password |
		| Al03@dontflymoney.com | password |
	And I have a ticket of this user
	And I pass this password
		| Current Password | Password     | Retype Password |
		| password         | new_password | password_wrong  |
	When I try to change the password
	Then I will receive this core error: RetypeWrong
	And the password will not be changed
	And the ticket will still be valid


Scenario: 99. Password change with info all right (S)
	Given I have this user created and activated
		| Email                 | Password |
		| Al99@dontflymoney.com | password |
	And I have a ticket of this user
	And I pass this password
		| Current Password | Password     | Retype Password |
		| password         | new_password | new_password    |
	When I try to change the password
	Then I will receive no core error
	And the password will be changed
	And the ticket will not be valid anymore