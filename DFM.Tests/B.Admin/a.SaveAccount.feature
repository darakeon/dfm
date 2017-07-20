Feature: a. Creation of Account

Background:
	Given I have an user

Scenario: 01. Save Account without name (E)
	Given I have this account to create
		| Name | Yellow | Red |
		|      |        |     |
	When I try to save the account
	Then I will receive this error
		| Error               |
		| AccountNameRequired |
	And the account will not be saved

Scenario: 02. Save Account with just yellow limit (E)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM | 100    |     |
	When I try to save the account
	Then I will receive this error
		| Error                    |
		| AccountTwoLimitsRequired |
	And the account will not be saved

Scenario: 03. Save Account with just red limit (E)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM |        | 100 |
	When I try to save the account
	Then I will receive this error
		| Error                    |
		| AccountTwoLimitsRequired |
	And the account will not be saved

Scenario: 04. Save Account with red limit bigger than yellow limit (E)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM | 100    | 200 |
	When I try to save the account
	Then I will receive this error
		| Error                    |
		| RedLimitAboveYellowLimit |
	And the account will not be saved

Scenario: 05. Save Account with name that already exists (E)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM |        |     |
	And I already have created this account
	When I try to save the account
	Then I will receive this error
		| Error                |
		| AccountAlreadyExists |
	And the account will not be saved

Scenario: 98. Save Account with info all right (without limits) (S)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM |        |     |
	When I try to save the account
	Then I will receive no error
	And the account will be saved

Scenario: 99. Save Account with info all right (with limits) (S)
	Given I have this account to create
		| Name       | Yellow | Red |
		| AccountDFM | 100    | 200 |
	When I try to save the account
	Then I will receive no error
	And the account will be saved
