Feature: b. Year report

Background:
	Given I have an user
	And I have an account
	And I have moves of
		| Date       |
		| 27/03/2012 |
		| 28/03/2012 |
		| 29/03/2012 |
		| 30/03/2012 |
		| 31/03/2012 |
		| 01/04/2012 |
		| 02/04/2012 |
		| 03/04/2012 |
		| 04/04/2012 |
		| 05/04/2012 |
		| 06/04/2012 |

Scenario: 01. Get with invalid Account ID (E)
	Given I pass an invalid account ID
	And I pass this date
		| Year |
		| 2012 |
	When I try to get the year report
	Then I will receive this error
		| Error            |
		| InvalidAccountID |
	And I will receive no year report

Scenario: 02. Get with Date Year Zero (E)
	Given I pass a valid account ID
	And I pass this date
		| Year |
		| 0    |
	When I try to get the year report
	Then I will receive this error
		| Error       |
		| InvalidYear |
	And I will receive no year report



Scenario: 99. Get with info all right (S)
	Given I pass a valid account ID
	And I pass this date
		| Year |
		| 2012 |
	When I try to get the year report
	Then I will receive no error
	And I will receive the year report
	And its sum value will be equal to its months sum value
